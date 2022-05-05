# netcore-postgresql-stack
Here is example project using .net core, postgresql stack

# Start postgresql on windows
docker run -d `
	--name my-postgres `
	-e POSTGRES_PASSWORD=123456aA@ `
	-e PGDATA=/var/lib/postgresql/data/pgdata `
	-v F:/MyProjects/db:/var/lib/postgresql/data `
	-p 5432:5432 `
	postgres
