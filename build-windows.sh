#!/usr/bin/env bash

dotnet publish --configuration Release --output ./publish --runtime win-x64 --self-contained true
