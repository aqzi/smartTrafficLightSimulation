import re
import matplotlib.pyplot as plt;

#This code is very dirty. It is only used to make some graphs.
def main() :
    startReading = False
    counter = 0
    list1 = []

    filename = "Assets/Results/results2_normal.txt"

    file1 = open(filename, 'r')
    Lines = file1.readlines()

    for line in Lines:
        list1.append(line)

    list2 = []

    for item in list1:
        if(startReading): list2.append(item)

        if "experiment 1" in item: startReading = True
        if "experiment 2" in item: startReading = False

    roadsWithData = list2[0].split(". ")
    roadsWithDataList = [""] * 4

    for item in roadsWithData:
        item2 = item.split(": ")
        roadsWithDataList[int(item2[0]) - 1] = re.findall("\\((.*?)\\)", item2[1])

    finalList = []
    for i, item in enumerate(roadsWithDataList):
        tmp = []

        for data in item:
            element = data.split(", ")

            if "True" in element: counter += 1
            else: counter -= 1

            time = float(element[1])
            tmp.append([counter, time])
        
        counter = 0
        finalList.append(tmp)

    plt.figure(0)

    for i, item in enumerate(finalList):
        if i == 0 or i == 2:
            l1 = [0]
            l2 = [0]

            for element in item:
                l1.append(element[0])
                l2.append(element[1])

            name = "road " + str(i+1)

            plt.plot(l2, l1, label = name)

    plt.xlabel('time in seconds')
    plt.ylabel('amount of cars')

    plt.legend()
    plt.show()

    plt.figure(1)

    for i, item in enumerate(finalList):
        if i == 1 or i == 3:
            l1 = [0]
            l2 = [0]

            for element in item:
                l1.append(element[0])
                l2.append(element[1])

            name = "road " + str(i+1)

            plt.plot(l2, l1, label = name)

    plt.xlabel('time in seconds')
    plt.ylabel('amount of cars')

    plt.legend()
    plt.show()

    if "normal" in filename: return

    decisions = list2[1].split(", ")

    for i, item in enumerate(decisions):
        if "road_1_3_green_fast" in item: decisions[i] = "1_3_fast"
        if "road_2_4_green_fast" in item: decisions[i] = "2_4_fast"
        if "road_1_3_green" in item: decisions[i] = "1_3_green"
        if "road_2_4_green" in item: decisions[i] = "2_4_green"

    plt.figure(2)

    plt.plot([element * 3 for element in list(range(0, len(decisions)))], decisions)

    plt.xlabel('time in seconds')
    plt.ylabel('prediction', labelpad=-70)

    plt.show()

main()