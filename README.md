API que realiza CRUD de Pilotos, Equipes e Grandes Prêmios e monta um relatório geral com o total de pilotos, equipes e grandes prêmios, calcula a idade média dos pilotos
e apresenta o piloto e a equipe com mais vitórias registradas. Os dados são salvos utilizando um banco de dados PostgreSQL, com três tabelas (Pilotos, Equipes, Grandes Prêmios).
Nesse contexto, um piloto pode vencer um ou mais grandes prêmios, mas um grande prêmio só pode ser vencido por um piloto (1:n); uma equipe pode ter um ou mais pilotos, 
mas o piloto pode ser associado somente a uma equipe (1:n);

Foi utilizado a estrutura de uma API Web do ASP.NET Core, considerando os pacotes Microsoft.EntityFramework, AutoMapper, Newtonsoft.Json, Swashbucle.AspNetCore, 
Npgsql.EntityFrameworkCore.PostgreSQL e xUnit para algumas tarefas, como: criação e comunicação com o banco, mapeamento entre modelos e dtos, criação da documentação da API 
via Swagger e para a criação de testes unitários. Espera-se utilizar outras bibliotecas (Mock, Bogus, AutoFixture) para melhorar a implementação dos testes de unidade (90% de cobertura).

Durante o desenvolvimento da aplicação, até o momento, pude aprofundar os conhecimentos do padrão REST, aprender a estruturar um projeto de API, aprender novas funcionalidades 
das bibliotecas, aprender a lógica por trás de modelos, controladores e serviços, além de aprofundar meus conhecimentos em testes unitários.