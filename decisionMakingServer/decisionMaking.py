import io
import socket
import PIL.Image as Image
import pickle
from PIL.ExifTags import TAGS

HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 65432        # Port to listen on (non-privileged ports are > 1023)

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    conn, addr = s.accept()
    with conn:
        # print('Connected by', addr)
        # while True:
        #     data = conn.recv(1024)

        #     if not data:
        #         break

        # conn.sendall(data)

        #-------------------

        with open('decisionMakingServer/img/tst.png', 'wb') as img:
            while True:
                data = conn.recv(1024)

                if not data:
                    break
                
                img.write(data)

        #---------------------

        # fullMsg = b''

        # while True:
        #     data = conn.recv(1024)
        #     print(data)
        #     print(data.decode());

        #     if not data:
        #         print("------------------------")
        #         break
        #     else: fullMsg += data

        # conn.sendall(data)
