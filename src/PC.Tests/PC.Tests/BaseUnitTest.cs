using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Ninject;
using Ninject.MockingKernel.Moq;

using PebbleCode.Entities;
using PebbleCode.Framework;
using PebbleCode.Framework.IoC;
using Ninject.Modules;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Tests
{
    /// <summary>
    /// A base class for unit tests - mocks out all database access in singleton scope
    /// </summary>
    [TestFixture]
    public abstract class BaseUnitTest<THelper> : BaseTest<THelper>
        where THelper : TestHelper, new()
    {
        protected MoqMockingKernel _mockContainer = null;
        
        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseUnitTest()
        {
        }

        /// <summary>
        /// Initialiser for unit tests
        /// </summary>
        [SetUp]
        public override void TestInitialise()
        {
            // Execute base initialisation
            base.TestInitialise();

            // Override the dependency injection container to use Moq container
            NinjectSettings settings = new NinjectSettings
            {
                InjectAttribute = typeof(PebbleCode.Framework.IoC.InjectAttribute)
            };
            _mockContainer = new MoqMockingKernel(settings);

            // Load all modules defined in this test assembly
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetExecutingAssembly()); // this one

            // Add in the assembly for each layer of the class hierarchy
            // (e.g for CT, we'll load CT.Tests and CT.Tests.Unit)
            Type type = this.GetType();
            while (type != null && type != typeof(BaseUnitTest<THelper>))
            {
                if (!assemblies.Contains(type.Assembly))
                    assemblies.Add(type.Assembly);
                type = type.BaseType;
            }

            // Load any that contain modules...
            foreach (Assembly ass in assemblies)
            {
                if (ass.GetTypes().Any(t => typeof(NinjectModule).IsAssignableFrom(t)))
                    _mockContainer.Load(ass);
            }

            //assemblies.ForEach(ass => _mockContainer.Load(ass));
            _mockContainer.Reset();

            //Make this kernel available to all other libraries
            Kernel.Instance = _mockContainer;
        }

        /// <summary>
        /// CleanUp for unit tests
        /// </summary>
        [TearDown]
        public override void TestCleanup()
        {
            // Deactivate any mocked objects
            //_mockContainer.Reset();
            //_mockContainer.Dispose();   

            // Allow base to cleanup
            base.TestCleanup();
        }

        /// <summary>
        /// Automatically wires up the indicated repository to return the collection 
        /// of mock/stub objects for all methods of type EListType.  Additionally, 
        /// any methods of type EType will return the first item in the collection
        /// </summary>
        /// <typeparam name="RepositoryType">The repository type to wire up</typeparam>
        /// <typeparam name="EType">BO type of the repository</typeparam>
        /// <typeparam name="EListType">BO collection type</typeparam>
        /// <param name="entitiesToReturn">Mock/stub objects already configured</param>
        /// <returns>The mock repository wrapper for further configuration</returns>
        protected Mock<RepositoryType> AutoWireUpMockRepository<RepositoryType, EType, EListType>(params EType[] entitiesToReturn)
            where RepositoryType : class
            where EType : class
            where EListType : List<EType>, new()
        {
            //Use the MockingKernel to get a Mock repository object from the DI container
            Mock<RepositoryType> mockRepo = _mockContainer.GetMock<RepositoryType>();
            
            //Search for repo accessor methods
            typeof(RepositoryType).GetMethods()
                .Where(mi => mi.Name.StartsWith("Get"))
                .ForEach(mi =>
                {
                    //Build a lamba expression in the format expected by the Moq, e.g.
                    //  mockRepo.Setup(repo => repo.Getxxxxx(It.IsAny<T>, It.IsAny<T>, etc...))
                                        
                    //the parameter for the lambda will therefore be a repository
                    var parameter = Expression.Parameter(typeof(RepositoryType));

                    //The body will be a function call on the repo that matches this particular method
                    var body = Expression.Call(parameter, mi,

                        //Since we don't care what parameters are passed (for this simple repo mock)
                        //match each parameter to a generic call of It.IsAny<ParamType>
                        mi.GetParameters().Select(pi =>
                            Expression.Call(typeof(It), "IsAny", new Type[] { pi.ParameterType })));

                    if (mi.ReturnType.Equals(typeof(EListType)))
                    {
                        //For collection types, return all indicated items
                        var lambdaExpression = Expression.Lambda<Func<RepositoryType, EListType>>(body, parameter);
                        EListType newCollection = new EListType();
                        newCollection.AddRange(entitiesToReturn);
                        mockRepo.Setup(lambdaExpression).Returns(newCollection);
                    }
                    else if (mi.ReturnType.Equals(typeof(EType)))
                    {
                        //For accessors returning a single item, pick the first item in the collection
                        var lambdaExpression = Expression.Lambda<Func<RepositoryType, EType>>(body, parameter);
                        mockRepo.Setup(lambdaExpression).Returns(
                            entitiesToReturn.Length == 0
                            ? (EType)null
                            : entitiesToReturn.First());
                    }
                });

            return mockRepo;
        }



        /// <summary>
        /// Automatically wires up the indicated repository to return the given entity 
        /// for all methods of type EType called with the entities ID
        /// </summary>
        /// <typeparam name="RepositoryType">The repository type to wire up</typeparam>
        /// <typeparam name="EType">BO type of the repository</typeparam>
        /// <param name="entityToReturn">Mock/stub object already configured</param>
        /// <returns>The mock repository wrapper for further configuration</returns>
        protected Mock<RepositoryType> AutoWireUpMockRepositoryGetById<RepositoryType, EType>(EType entityToReturn)
            where RepositoryType : class
            where EType : Entity
        {
            //Use the MockingKernel to get a Mock repository object from the DI container
            Mock<RepositoryType> mockRepo = _mockContainer.GetMock<RepositoryType>();

            //Search for repo accessor methods
            typeof(RepositoryType).GetMethods()
                .Where(mi => mi.Name.StartsWith("Get"))                                             // Get methods
                .Where(mi => mi.GetParameters().ToList().Exists(pi => pi.Name.ToLower() == "id"))   // With an id parameter
                .Where(mi => mi.ReturnType.Equals(typeof(EType)))                                   // return an single entity
                .ForEach(mi =>
                {
                    //Build a lamba expression in the format expected by the Moq, e.g.
                    //  mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i.Equals(idToIntercept)), It.IsAny<T>(), etc...))

                    //the parameter for the lambda will therefore be a repository
                    var parameter = Expression.Parameter(typeof(RepositoryType));

                    // build up param list
                    List<Expression> arguments = new List<Expression>();

                    foreach (ParameterInfo pi in mi.GetParameters())
                    {
                        if (pi.Name.ToLower() == "id")
                        {
                            // Nested lambda:  i => i.Equals(idToIntercept)
                            ParameterExpression i = Expression.Parameter(typeof(int));
                            ConstantExpression value = Expression.Constant(entityToReturn.Identity);
                            MethodCallExpression idEqualsBody = Expression.Call(i, "Equals", null, value);
                            
                            //Older version of Moq
                            //var idEqualsParam = Expression.Lambda<Predicate<int>>(idEqualsBody, i);

                            //Newer version of Moq, swap the above for this, more consistent use of Func delegate type in place of deprecated Predicate
                            var idEqualsParam = Expression.Lambda<Func<int, bool>>(idEqualsBody, i);
                            
                            arguments.Add(Expression.Call(typeof(It), "Is", new Type[] { pi.ParameterType }, idEqualsParam));
                        }
                        else
                            // It.IsAny<T>()
                            arguments.Add(Expression.Call(typeof(It), "IsAny", new Type[] { pi.ParameterType }));
                    }

                    // The body will be a function call on the repo that matches this particular method
                    var body = Expression.Call(parameter, mi, arguments);

                    // Setup the mock to return the entity
                    var lambdaExpression = Expression.Lambda<Func<RepositoryType, EType>>(body, parameter);
                    mockRepo.Setup(lambdaExpression).Returns(entityToReturn);
                });

            return mockRepo;
        }

        protected void AutoWireUpMockRepositorySave<RepositoryType, EType>(Action<IEnumerable<EType>> callback)
            where RepositoryType : class
            where EType : Entity
        {
            //Use the MockingKernel to get a Mock repository object from the DI container
            Mock<RepositoryType> mockRepo = _mockContainer.GetMock<RepositoryType>();

            //Need to create a callback wrapper to support repo.Save(It.Is<Flags>(), It.IsAny<EListType>())
            var wrappedCallback = new Action<Flags, IEnumerable<EType>>(
                (flags, savedItems) => callback(savedItems)
                    );
            
            //Search for repo accessor methods
            typeof(RepositoryType).GetMethods()
                .Where(mi => mi.Name.Equals("Save"))
                .ForEach(mi =>
                    {
                        //1. Build a lamba expression in the format expected by the Moq, e.g.
                        //  mockRepo.Setup(repo => repo.Save(It.Is<Flags>(), It.IsAny<EListType>()))

                        //the parameter for the lambda will be a repository
                        var parameter = Expression.Parameter(typeof(RepositoryType));

                        //The body will be a function call on the repo that matches this particular method
                        var body = Expression.Call(parameter, mi,

                            //Since we don't care teh value of parameters are passed
                            //match each parameter to a generic call of It.IsAny<ParamType>
                            mi.GetParameters().Select(pi =>
                                Expression.Call(typeof(It), "IsAny", new Type[] { pi.ParameterType })));

                        var lambdaExpression = Expression.Lambda<Action<RepositoryType>>(body, parameter);

                        //2. Instruct Moq to intercept calls that match this pattern, and perform `callback`
                        if (mi.GetParameters().First().ParameterType.Equals(typeof(Flags)))
                        {
                            mockRepo.Setup(lambdaExpression).Callback(wrappedCallback);
                        }
                        else
                        {
                            mockRepo.Setup(lambdaExpression).Callback(callback);
                        }
                    });
        }

        /// <summary>
        /// Grab an auto incrementing entity id
        /// </summary>
        protected int NextEntityId
        {
            get { return _entityIdCounter++; }
        }
        
        private static int _entityIdCounter = 1;
    }
}
