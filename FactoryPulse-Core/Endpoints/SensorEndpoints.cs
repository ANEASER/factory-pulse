namespace FactoryPulse_Core.Endpoints;

public static class SensorEndpoints
{
    public static RouteGroupBuilder MapSensorEndpoints(this WebApplication app)
    {
        const string ListnToSensorsName = "ListenToSensors";

        var group = app.MapGroup("sensors");

        group.MapGet("/", () => Results.Content("""
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>Factory Sensor Dashboard</title>
                        <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js"></script>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 20px; background: #f5f5f5; }
                            .dashboard { display: grid; grid-template-columns: repeat(auto-fit, minmax(400px, 1fr)); gap: 20px; }
                            .machine-card { background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
                            .machine-title { font-size: 1.5em; margin-bottom: 15px; color: #333; }
                            .metric { display: flex; justify-content: space-between; margin: 10px 0; padding: 8px; background: #f8f9fa; border-radius: 4px; }
                            .metric-name { font-weight: bold; }
                            .status-OK { color: green; font-weight: bold; }
                            .status-WARN { color: orange; font-weight: bold; }
                            .status-ERROR { color: red; font-weight: bold; }
                            .timestamp { color: #666; font-size: 0.9em; text-align: center; margin-top: 10px; }
                        </style>
                    </head>
                    <body>
                        <h1> Factory Sensor Dashboard</h1>
                        <div id="dashboard" class="dashboard"></div>

                        <script>
                            const connection = new signalR.HubConnectionBuilder()
                                .withUrl("/sensorHub")
                                .build();

                            const machines = {};

                            connection.start().then(() => {
                                console.log("Connected to SignalR hub");
                            }).catch(err => console.error(err));

                            connection.on("SensorUpdate", (machine, metric, value) => {
                                if (!machines[machine]) {
                                    machines[machine] = {};
                                    createMachineCard(machine);
                                }
                                
                                machines[machine][metric] = value;
                                machines[machine].lastUpdate = new Date().toLocaleTimeString();
                                updateMachineCard(machine);
                            });

                            function createMachineCard(machine) {
                                const dashboard = document.getElementById('dashboard');
                                const card = document.createElement('div');
                                card.className = 'machine-card';
                                card.id = `machine-${machine}`;
                                card.innerHTML = `
                                    <div class="machine-title">${machine.toUpperCase()}</div>
                                    <div class="metrics"></div>
                                    <div class="timestamp"></div>
                                `;
                                dashboard.appendChild(card);
                            }

                            function updateMachineCard(machine) {
                                const card = document.getElementById(`machine-${machine}`);
                                const data = machines[machine];
                                
                                const metricsHtml = `
                                    <div class="metric">
                                        <span class="metric-name"> Temperature:</span>
                                        <span>${data.temperature || '--'}°C</span>
                                    </div>
                                    <div class="metric">
                                        <span class="metric-name"> Vibration:</span>
                                        <span>${data.vibration || '--'}g</span>
                                    </div>
                                    <div class="metric">
                                        <span class="metric-name"> Pressure:</span>
                                        <span>${data.pressure || '--'} psi</span>
                                    </div>
                                    <div class="metric">
                                        <span class="metric-name"> Humidity:</span>
                                        <span>${data.humidity || '--'}%</span>
                                    </div>
                                    <div class="metric">
                                        <span class="metric-name"> RPM:</span>
                                        <span>${data.rpm || '--'}</span>
                                    </div>
                                    <div class="metric">
                                        <span class="metric-name"> Status:</span>
                                        <span class="status-${data.status || 'OK'}">${data.status || 'OK'}</span>
                                    </div>
                                `;
                                
                                card.querySelector('.metrics').innerHTML = metricsHtml;
                                card.querySelector('.timestamp').innerHTML = `Last update: ${data.lastUpdate}`;
                            }
                        </script>
                    </body>
                    </html>
                    """, "text/html")).WithName(ListnToSensorsName);

        group.MapGet("/test-error", () =>
        {
            throw new InvalidOperationException("This is a test exception!");
        });

        return group;
    }
}