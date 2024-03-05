import socket
import time

host, port = "127.0.0.1", 25001
comm_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
comm_socket.connect((host, port))

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