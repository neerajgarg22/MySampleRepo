This project contains .Net Core Web API(Product Service) with database as SQL Server and Redis Cache. 

In this project, while fetching the product by Id, it will first check in Redis cache, if product already exists in cache, it will return the data from cache itself. 
Otherwise it will fetch from Database and then add it to cache. 

It also contains docker-compose.yml file to run all 3 aforementioned services. 
Just download the docker-compose.yml file and run below command - 

docker-compose up --build -d

After running the command, access Product Service Swagger from below URL to run APIs - 

http://localhost:5000/swagger/index.html
