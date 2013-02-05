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
