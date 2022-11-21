#!/bin/sh

echo "$API" >> /proc/1/fd/1

echo "Looking for appsettigns in /usr/share/nginx/html/" >> /proc/1/fd/1
find "/usr/share/nginx/html/" -follow -type f -name "appsettings.json*" -print | sort -V | while read -r f; do
    case "$f" in
        *) echo "File $f">> /proc/1/fd/1;
        tempFile=$f".temp";
        echo "Temp file $tempFile">> /proc/1/fd/1;
        envsubst < $f > $tempFile;
        cp -f $tempFile $f;
        rm $tempFile;
    esac
done

gzip -c appsettings.json > appsettings.json.gz