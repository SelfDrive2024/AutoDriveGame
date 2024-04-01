# reference: https://www.datacamp.com/tutorial/a-complete-guide-to-socket-programming-in-python

import socket
import time

host, port = "127.0.0.1", 25001
comm_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
comm_socket.connect((host, port))

comm_socket.listen(0)
print(f"Listening on {host}:{port}")

# accept incoming connections
client_socket, client_address = comm_socket.accept()
print(f"Accepted connection from {client_address[0]}:{client_address[1]}")

# receive data from the client
while True:
    request = client_socket.recv(2048)
    request = request.decode("utf-8") # convert bytes to string
    
    # if we receive "close" from the client, then we break
    # out of the loop and close the conneciton
    if request.lower() == "close":
        # send response to the client which acknowledges that the
        # connection should be closed and break out of the loop
        client_socket.send("closed".encode("utf-8"))
        break

    print(f"Received: {request}")

def sendInstuction(instruction):
    instruction = str(instruction)
    comm_socket.sendall(instruction.encode("UTF-8"))

while True:
    if (input() == "a"):
        sendInstuction("A")
    elif (input() == "d"):
        sendInstuction("D")
    elif (input() == "s"):
        sendInstuction("S")
    else:
        sendInstuction("W")
