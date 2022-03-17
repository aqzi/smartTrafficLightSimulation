import matplotlib.pyplot as plt;
import numpy as np
from operator import itemgetter
from create_graph import readFile

#visualize leaving cars
def main() :
    list1 = []

    readFile('Assets/Results/leavingCars.txt', list1)

    list2 = []
    for item in list1: list2.append((int(item[0]) * 3600) + (int(item[1]) * 60) + (int(item[2])))

    list2.sort()

    substractAmount = list2[0]
    list3 = []
    for item in list2: list3.append(item - substractAmount)

    list5 = [0] * 60

    for item in list3:
        if item != 0: list5[item] = list5[item] + 1

    for i, item in enumerate(list5):
        if(i != 0): list5[i] = list5[i] + list5[i-1]

    plt.plot(list(range(0, 60)), list5, label = "test")

    plt.xlabel('time in seconds')
    plt.ylabel('amount of leaving cars')

    plt.legend()
    plt.show()

main()