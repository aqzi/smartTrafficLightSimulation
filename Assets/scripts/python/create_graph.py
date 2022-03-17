def readFile(file, list1) :
    f = open(file, 'r')
    Lines = f.readlines()

    for line in Lines:
        l = line.split(", ")
        for i in l: 
            if "\n" in i:
                n = i.replace("\n", "")
                list1.append(n.split(":"))
            else:
                list1.append(i.split(":"));