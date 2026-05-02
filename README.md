\# Plataforma de Ejecución, Trazabilidad y Comparación de Métodos Numéricos



Proyecto final del curso \*\*021 - Métodos Numéricos\*\* de la Universidad Mariano Gálvez de Guatemala.



\## Objetivo del proyecto



Desarrollar una aplicación web que permita configurar, ejecutar y analizar métodos numéricos mediante jobs, guardando su estado, parámetros, resultados e iteraciones.



La arquitectura final planificada incluye:



\- API principal en .NET 10

\- Worker numérico en Python

\- Redis como cola de trabajos

\- SQL Server como base de datos

\- Frontend SPA con React + TypeScript + Vite

\- Docker Compose para levantar todos los servicios



\## Estado actual del proyecto



Avance actual de la Fase 1 local:



\- API .NET creada.

\- Swagger funcionando.

\- Modelos `Job` y `JobIteration` creados.

\- DTO `CreateJobRequest` creado.

\- Entity Framework Core configurado.

\- SQLite temporal configurado para desarrollo local.

\- Migración inicial creada.

\- Base de datos local `jobs.db` creada.

\- Endpoints core implementados:

&#x20; - `POST /jobs`

&#x20; - `GET /jobs`

&#x20; - `GET /jobs/{id}`

&#x20; - `GET /jobs/{id}/iterations`

\- Endpoint temporal de simulación:

&#x20; - `POST /jobs/{id}/simulate`

\- Estructura base de `worker-python` creada.

\- Frontend base creado con React + TypeScript + Vite.



\## Endpoints disponibles



\### Crear job



```http

POST /Jobs





Ejemplo de Body:

JSON

{

&#x20; "method": "newton-raphson",

&#x20; "parameters": {

&#x20;   "function": "x^3 - x - 2",

&#x20;   "x0": 1.5,

&#x20;   "tolerance": 0.0001,

&#x20;   "maxIterations": 20

&#x20; }

}



Listar Jobs

GET /Jobs



Tambier permite filtros:

GET /jobs?status=DONE\&method=newton-Raphson





Consultar JOB por ID

GET /jobs/{id}



consultar iteración de un JOB

GET /jobs/{id}/iterations



Simular ejecución de un Job

POST /jobs/{id}/simulate

