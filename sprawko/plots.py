import matplotlib.pyplot as plt
import numpy as np

files = np.array([2835154]*9)

x = [2**y for y in range(0, 9)]

hdd = [149.83, 110.02, 108.61, 102.38, 112.52, 124.47, 139.83, 135.01, 138.10]
nvme = [36.14, 19.0, 11.07, 7.19, 6.0, 6.06, 6.08, 6.15, 6.35]
pen3_2 = [39.68, 25.27, 25.33, 22.57, 23.57, 22.46, 22.72, 25.15, 24.03]
pen2_0 = [87.22, 66.68, 58.99, 54.05, 57.75, 63.22, 66.49, 66.63, 67.44]

speed_hdd = files / np.array(hdd)
speed_nvme = files / np.array(nvme)
speed_pen3_2 = files / np.array(pen3_2)
speed_pen2_0 = files / np.array(pen2_0)

fig, ax = plt.subplots()
plt.xscale('log', base=2)
plt.xticks(x,x)
ax.plot(x, hdd, label="HDD")
ax.plot(x, nvme, label="NVMe")
ax.plot(x, pen3_2, label="Pendrive USB 3.2")
ax.plot(x, pen2_0, label="Pendrive USB 2.0")
plt.xlabel("Liczba wątków")
plt.ylabel("Czas działania programu")
ax.legend()
plt.savefig("time.png")

fig, ax = plt.subplots()
plt.xscale('log', base=2)
plt.xticks(x,x)
ax.plot(x, speed_hdd, label="HDD")
ax.plot(x, speed_nvme, label="NVMe")
ax.plot(x, speed_pen3_2, label="Pendrive USB 3.2")
ax.plot(x, speed_pen2_0, label="Pendrive USB 2.0")
plt.xlabel("Liczba wątków")
plt.ylabel("Prędkość działania [pliki/s]")
ax.legend()
plt.savefig("speed.png")