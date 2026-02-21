🧠 Domain (Coração do sistema) aqui ficam as :
- Entidades : Representação dos objetos do mundo real que fazem parte do sistema, como Equipamento, Usuário, etc. As entidades possuem atributos e comportamentos que definem suas características e ações dentro do sistema.
- Values Objects : Representação de objetos que possuem valor, mas não possuem identidade própria, como Endereço, Data, etc. Os Value Objects são imutáveis e são utilizados para representar conceitos que não possuem uma identidade única.
- Regras de Negócio Puras : Representação das regras de negócio que governam o comportamento do sistema, como validações, cálculos, etc. As regras de negócio puras são aquelas que não dependem de infraestrutura ou de outras camadas do sistema, e podem ser testadas de forma isolada. Elas são responsáveis por garantir que o sistema funcione de acordo com as regras definidas para o domínio.
- Interfaces : Representação das interfaces que definem os contratos para as operações do sistema, como repositórios, serviços, etc. As interfaces são utilizadas para abstrair a implementação das operações e permitir a flexibilidade na escolha das tecnologias e frameworks utilizados na infraestrutura. Elas são responsáveis por definir as operações que podem ser realizadas no domínio e garantir que as implementações sigam os contratos definidos.
- Eventos de Domínio : Representação dos eventos que ocorrem dentro do domínio, como criação de um equipamento, atualização de um usuário, etc. Os eventos de domínio são utilizados para notificar outras partes do sistema sobre mudanças que ocorreram no domínio, permitindo a comunicação assíncrona e a integração entre diferentes componentes do sistema. Eles são responsáveis por garantir que as mudanças no domínio sejam propagadas de forma eficiente e consistente para outras partes do sistema.

Domain não é capaz de reconhecer alguns componetes do projeto como:
	
	❌ Não conhece banco
	
	❌ Não conhece Rabbit

	❌ Não conhece HTTP

	❌ Não conhece EF

	❌ Não conhece nada externo

Ele só conhece regra de negócio.

"Sem referências a Projetos externos"