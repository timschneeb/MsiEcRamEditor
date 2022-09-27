# MsiEcRamEditor
Edit and dump the RAM of the EC chip of MSI laptops via ACPI-WMI on Windows. Tested with MSI Modern 15 A11M.

Precompiled Windows binary: https://github.com/ThePBone/MsiEcRamEditor/releases

### Alternative Linux method
On Linux, you can directly monitor EC RAM using this command instead:
```bash
sudo watch -n 0.1 hexdump -C /sys/kernel/debug/ec/ec0/io
```
If the path is unavailable, install an kernel module like: https://github.com/musikid/acpi_ec (and substitude path with `/dev/ec` if required).

To write to memory, I used [ISW-Modern](https://github.com/FaridZelli/isw-modern) on Linux:
```bash
sudo isw -s <addr> <value>
```
