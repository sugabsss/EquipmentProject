📦 Infrasctructure (RabbitMQ):

É infraestrutura : Representação do meu sistema de Gerênciamento de filas utilizando RabbitMQ, um sistema de mensagens que permite a comunicação assíncrona entre diferentes partes do sistema. Ele é utilizado para gerenciar as filas de mensagens, garantindo que as mensagens sejam entregues de forma confiável e eficiente. O RabbitMQ é uma escolha popular para sistemas de gerenciamento de filas devido à sua robustez, escalabilidade e suporte a diversos protocolos de mensagens.

	📤 Publisher : Responsável por enviar mensagens para as filas do RabbitMQ. Ele é utilizado para publicar mensagens que precisam ser processadas por outros componentes do sistema. O Publisher é responsável por garantir que as mensagens sejam formatadas corretamente e enviadas para a fila correta.

	📥 Consumer : Responsável por receber mensagens das filas do RabbitMQ. Ele é utilizado para consumir mensagens que foram publicadas pelo Publisher e processá-las de acordo com a lógica de negócios do sistema. O Consumer é responsável por garantir que as mensagens sejam processadas de forma eficiente e que os resultados sejam retornados corretamente.

"Referência Domain e Application"