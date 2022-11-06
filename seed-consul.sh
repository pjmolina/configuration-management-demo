#!/bin/sh   
# Add initial data to consul K/V store

curl -i --request PUT http://localhost:8500/v1/kv/acme/system-b/port --data '90'
curl -i --request PUT http://localhost:8500/v1/kv/acme/energy/port --data '80'
curl -i --request PUT http://localhost:8500/v1/kv/acme/energy/db --data '"database1"'
curl -i --request PUT http://localhost:8500/v1/kv/acme/energy/features/a --data 'true'
curl -i --request PUT http://localhost:8500/v1/kv/acme/energy/features/b --data 'false'
curl -i --request PUT http://localhost:8500/v1/kv/acme/energy/shared-secret1 --data '"abracadabra"'
