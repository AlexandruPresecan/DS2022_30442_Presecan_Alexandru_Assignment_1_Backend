In order to run the application, Docker Desktop must first be installed.
The latest postgres database must be retrieved using the following command: docker pull postgres.
A docker image for backend can be creating using the command: docker build -t backend [path to backend folder containing docker file].
A docker image for frontend can be creating using the command: docker build -t frontend [path to frontend folder containing docker file].
Finally, to run the docker compose, run the following command from the folder containing the docker compose file: docker-compose up -d.
