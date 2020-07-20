import donut
import sys
import os
import ntpath
from base64 import b64encode as b64e

# PARSE SCRIPT ARGUMENTS

if len(sys.argv) <= 1:
    print("\n[!] Please provide .NET binary to generate shellcode and specify arch (optionally)!")
    print("    Usage:\n\t{} agent.exe [x64]".format(sys.argv[0]))
    sys.exit(1)

donut_id = 3
arch = "anyCPU"
try:
    if sys.argv[2] == "x64":
        arch = "x64"
        donut_id = 2
    elif sys.argv[2] == "x86":
        arch = "x86"
        donut_id = 1
except:
    pass


# PARSE FILENAME (TAKES FULL PATH AND CURRENT DIR)

filename = os.fspath(sys.argv[1])
print(f"[!] Generating {arch} shellcode from file:\n    {filename}")
files = [f for f in os.listdir('.') if os.path.isfile(f)]

if filename not in files:
    sc = donut.create(file=filename,arch=donut_id)
    if sc is None:
        print(f"\n[-] Cannot find file:\n    {filename}\n\
    Ensure that file is either in current directory or full path is specified!")
        sys.exit(1)
else:
    sc = donut.create(file=filename,arch=donut_id)


# WRITE SHELLCODE
head,tail = ntpath.split(filename)
sc_filename = tail.split('.')[0]+'{}'.format(arch)+'.bin'
sc_filepath = os.path.join(os.getcwd(),sc_filename)
filesc = open(sc_filepath,'wb')
length = filesc.write(sc)
filesc.close()
print(f"[+] Shellcode of {length} bytes written in:\n    {sc_filepath}")

# WRITE BASE64 VERSION
fileb = open(sc_filepath+'.b64','w')
fileb.write(b64e(sc).decode())
fileb.close()
print(f"[+] Base64 version is written to:\n    {sc_filepath}.b64")
