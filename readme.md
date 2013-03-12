### PC.ServiceBus

This project is used for sending and receiving messages from Azure Service Bus. The main classes to understand are:

1. **EventBus:**
    This class is used for publishing events to a Topic. The code is the same as CommandBus except the verb is Publish and not Send.
    This is because you have more than one handler registered per event.
2. **CommandBus:**
    This class is used for sending commands to a Topic. The code is the same as the EventBus except the verb is Send.
    This is because you can only have one handler for a command.
3. **TopicSender:**
    This class handles sending BrokeredMessages to a Topic. Within it is the retry logic required to handle transient faults.
4. **SubscriptionReceiver:**
    This class handles receiving BrokeredMessages from a subscription. Within it is the retry logic required to handle transient faults.

An example config section within the app.config file of the service you want to use this component looks like the following:

```xml
  <azureConfiguration>
    <settings>
      <add key="azureNamespace" value="[namespace]"></add>
      <add key="issuer" value="[issuer]"></add>
      <add key="key" value="[secretkey]"></add>
    </settings>
    <topics>
      <topic name="test">
        <subscription name="test"></subscription>
      </topic>
      <topic name="test" isEventBus="true">
        <subscription name="test"></subscription>
      </topic>      
    </topics>
  </azureConfiguration>
```

### PC.Monitoring

This project is used for adding monitoring endpoints into a service. There are currenty the following monitors

1. DatabaseServiceMonitor - Used to check the database server is up and running
2. DummyServiceMonitor - Used for currently running process
3. TcpPortServiceMonitor - USed to check a specific TCP port is active and accepting connections


An example of a config section within the app.config file of the service you want to monitor would look like the following,
this section is taking out of the host for Core:

```xml
<monitoringConfiguration>
    <serviceMonitorGroups>
      <serviceMonitorGroup name="SGP.Wallet">
          <serviceMonitors>    
              <serviceMonitorConfiguration name="SGP IGT" type="TcpPortServiceMonitor">
                  <settings>
                      <add key="ipAddress" value="localhost"/>
                      <add key="port" value="18200"/>
                      <add key="timeout" value="5"/>
                  </settings>
              </serviceMonitorConfiguration>
              <serviceMonitorConfiguration name="SGP Wallet" type="DummyServiceMonitor"/>
              <serviceMonitorConfiguration name="SGP MySql" type="DatabaseServiceMonitor">
                  <settings>
                      <add key="timeout" value="5"/>
                  </settings>
              </serviceMonitorConfiguration>
            </serviceMonitors>
        </serviceMonitorGroup>
    </serviceMonitorGroups>
</monitoringConfiguration>
```
