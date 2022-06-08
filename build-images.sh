#!/bin/sh
cd energy-spa
npm run build:prod
cd ..
docker compose build
