#!/bin/bash
set -e
sleep 20
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P Admin1234 -d master -i setup.sql