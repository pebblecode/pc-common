How to re-generate the entities and repository files used in the integration tests

1) Create a pc_test database in MySQL
2) Give access to the pctest user (password pctest)
3) Copy the DefaultSettings.xml file into the C:\program files (x86)\MyGeneration13\Settings folder
4) Map a P: drive to the root folder of the pc-common repository using SUBST P: C:\dev\pc-common
5) From the command line run:

C:\program files (x86)\MyGeneration13\ZeusCmd -l out.log -p pc_test.zprj

6) Profit