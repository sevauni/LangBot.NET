#!/bin/bash
#This script preapres folder structure for the Telegram Language Bot
#Futher info at github.com/sevauni



echo ""
echo ""
echo "This script prepares folder structure for the Telegram Language Bot"
echo "Additional info could be found at github.com/sevauni"
echo ""
echo ""
echo "You should have folowing folder structure:"
echo "--/path-to-www-folder/index.html"
echo "---------------------/imgs/1/1.jpg"
echo "---------------------/imgs/1/2.jpg"
echo "---------------------/.........../"
echo "---------------------/imgs/99/1.jpg"
echo "---------------------/imgs/99/2.jpg"
echo ""
echo ""
sleep 1

dir="$1"
[ $# -eq 0 ] && { echo "Usage: $0 dir-name"; exit 1; }
 
if [ -d "$dir" -a ! -h "$dir" ]
then
   echo "Imgs folder found! Startng renaming..."
else
   echo "Error: $dir not found or is symlink to $(readlink -f ${dir})."
fi



cd imgs

for d in */; do # only match directories
   cd "$d"
   pwd
   ls -v | cat -n | while read n f; do mv -n "$f" "$n.jpg"; done
   cd ..
done