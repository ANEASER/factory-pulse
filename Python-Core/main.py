# sensor_simulator.py

import paho.mqtt.client as mqtt
import random
import time

# MQTT Configuration
MQTT_BROKER = "localhost"
MQTT_PORT = 1883

# List of machines to simulate
machines = ["machine1", "machine2", "machine3"]

# Possible statuses
statuses = ["OK", "WARN", "ERROR"]

# Create MQTT client and connect to broker
client = mqtt.Client()
client.connect(MQTT_BROKER, MQTT_PORT)

# Infinite loop to publish sensor data
while True:
    for machine in machines:
        # Simulate sensor data
        temperature = random.uniform(20.0, 100.0)  # °C
        vibration = random.uniform(0.1, 5.0)       # g
        pressure = random.uniform(30.0, 120.0)     # psi
        humidity = random.uniform(20.0, 80.0)      # %
        rpm = random.randint(500, 3000)            # RPM
        status = random.choices(statuses, weights=[0.85, 0.1, 0.05])[0]  # mostly OK

        client.publish(f"factory/{machine}/temperature", f"{temperature:.2f}")
        client.publish(f"factory/{machine}/vibration", f"{vibration:.2f}")
        client.publish(f"factory/{machine}/pressure", f"{pressure:.2f}")
        client.publish(f"factory/{machine}/humidity", f"{humidity:.2f}")
        client.publish(f"factory/{machine}/rpm", str(rpm))
        client.publish(f"factory/{machine}/status", status)

        print(
            f"[{machine}] Temp={temperature:.2f}°C | Vib={vibration:.2f}g | "
            f"Pres={pressure:.2f}psi | Hum={humidity:.2f}% | RPM={rpm} | Status={status}"
        )

    time.sleep(10)
    print("-----------------------------------------------------------------------------------------")
