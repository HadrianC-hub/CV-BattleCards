# CV-BattleCards

Requisitos mínimos del programa:
- Tener una computadora (estrictamente necesario)
- Que funcione (opcional)
- Sistema Operativo: Windows 10
- Instalar software ".Net Core v6.0"

Instrucciones para el editor de cartas:

Usted puede crear y editar sus propias cartas dentro del juego modificando
el archivo Editor.txt dentro de la carpeta Edit y escribiendo el código del
lenguaje diseñado para ello que le proveemos a continuación.

Dado que nuestro especialista en nombres para objetos aleatorios está de 
viaje, hemos decidido llamar a un suplente que le ha otorgado a nuestro
lenguaje el nombre "Just_a_regular_language_based_in_C#"... Con esta
poderosa herramienta usted será capaz de crear una serie de cartas y
propiedades que le especificaremos a continuación:

-Cartas (Debe haber sido toda una sorpresa para usted el saber esto.
Cada carta contiene una serie de parámetros que deben ser llenados,
ellos son: Nombre (string) , Vida (int), Energía (int), Daño (int) )

-Acciones (Cada carta contiene un conjunto de no más de tres acciones,
las cuales podrán ser activadas durante el turno de ataque del jugador
que la posea en ese momento)

-Efectos (Así como una carta contiene acciones, cada acción contiene un
conjunto indefinido de efectos. Los efectos son instrucciones del programa
que se cumplen como resultado de realizar alguna acción, por ejemplo :
la acción Atacar contiene a los efectos BajarVida y QuitarEnergía, y cada
uno de estos efectos contiene un string que representa el comando específico
que debe ser interpretado por el programa para la ejecución del efecto)

-Condiciones (Las acciones contienen también condiciones, que pueden o no
cumplirse en dependencia del estado de la partida. Para que una acción pueda
ser activada, todas sus condiciones deben cumplirse al mismo tiempo. Cada
condición contiene un string que representa la interpretación del programa 
parseada a un booleano que cambia de valor en dependencia de un contexto
específico del juego)

Nuestro lenguaje permite programar desde cosas tan simples como un efecto de 
Curar que rellene la vida de la carta en una cantidad específica, hasta una
Acción que solo pueda ejecutarse si todas las cartas de la mano del adversario
tienen un ataque mayor al de mi carta, si al menos una carta de mi mano tiene
energía más baja que una carta objetivo en el campo o si en el campo existen
una cantidad específica de cartas. Al mismo tiempo es posible programar efectos
que reduzcan los parámetros del objetivo en una cantidad referenciada a otro 
parámetro de otra carta activadora, como el efecto Dañar, que reduce la vitalidad
del objetivo en la cantidad de puntos de ataque que tenga la carta activadora,
el efecto Persuadir, que reduce a 0 los puntos de ataque que tenga el objetivo
y el efecto Matar, que elimina todos los puntos de vida del objetivo. Es importante
destacar que es posible crear una condicional que se cumpla si alguno de sus valores
se cumple (lo equivalente a insertar un operador 'or') y en el caso del operador and
consideramos innecesario su aplicación en nuestro lenguaje, ya que gracias a la
funcionalidad de "todo o nada" de la activación de habilidades, crear una condición
que contenga dos valores dónde ambos tengan que cumplirse es equivalente a crear dos
condiciones.

A continuación expondremos ejemplos de código para mostrar la sintaxis del lenguaje
y el modo de uso:

card Soldier. { health: 100, energy: 50, damage:25, actions:(Attack)(Defend)(Selfheal),}

-card (declaración de la variable a crear (una carta))
-Soldier. (declaración del nombre que va a tomar esta carta en el juego) (se declara con un punto al final)
-{ (abrir llave para editar sus parámetros, es necesario llenar todos los parámetros que aparecen aquí)
- health:, energy:, damage: (parámetros de la carta, deben ser llenados con valores de tipo (int) )
-actions: (debe introducirse no más de 3 string que representan acciones separados por paréntesis y sin espacio
en blanco entre ellos)
-} (finaliza la edición y procede a declarar otro objeto)

action Selfheal. {conditions:(LifeLessThan50)(EnergyMoreThan15), effects:(Heal)(DecreaseEnergy),}

-action (declaración de la variable a crear (una acción))
-SelfHeal. (el nombre de la acción debe ser igual a la referencia que toma en la declaración de las cartas que
la contengan) (en este caso estamos implementando una acción, por tanto va con . al final)
-{ (abre la llave para la edición de los parámetros de la acción)
-conditions:, effects: (al igual que las acciones en la carta, las condiciones y efectos deben ser declarados por
nombre exacto y separados por paréntesis sin espacios entre ellos. Para terminar se separa con coma (,) )
-} (fin de la edición)

condition EnergyMoreThan15. { value: caster.energy=>15|caster.health<2*target.damage,}

-value (se define una línea de código a interpretar, donde son contenidos un miembro izquierdo, un separador
(<,=,>,<=,=>) y un miembro derecho. El miembro izquierdo debe ser siempre una referencia a un parámetro de una
carta específica o conjunto de cartas, y el miembro derecho puede ser una referencia, un número entero o un cálculo
matemático con operaciones básicas como multiplicación, división, suma y resta de números y referencias numéricas.)

Las expresiones válidas para el miembro izquierdo del valor de una condicional son:
•caster.(health,energy,damage) => referencia al valor actual del parámetro de la carta que activa la habilidad
•target.(health,energy,damage) => referencia al valor actual del parámetro de la carta que recibirá el ataque
•player.(anycard,allcards).(health,energy,damage) => referencia al valor actual del parámetro de cualquier carta 
 de la mano del jugador, o a todas las cartas de su mano.
•enemy.(anycard,allcards).(health,energy,damage) => similar al anterior, pero con las cartas del enemigo
•field.cards (cuenta la cantidad de cartas en el tablero del juego, y solo es comparable con un número entero entre 1 y 9.

Excepto para comparar con field.cards, cualquier expresión ya mencionada puede pertenecer al miembro derecho de una
comparación establecida como valores de una condicional. Esto incluye referencias de valores, números enteros y cálculos
con operaciones matemáticas simples.

effect Heal. {value: caster.health=(caster.health+25),}

-value: (a diferencia del valor de una condición, los valores de los efectos solo pueden ser expresiones binarias separadas
por un símbolo de igualdad (=) o lo que es lo mismo, solo pueden ser asignaciones a parámetros de las cartas caster y target,
por lo cual el miembro izquierdo solo debe contener una referencia a un parámetro que será modificado según el miembro derecho
del valor. El miembro derecho, por otro lado, no varía en comparación con el miembro derecho de las condiciones)

Esto es todo sobre el lenguaje de programación. Cabe destacar que en la carpeta edit del juego encontrará dos archivos .txt
Uno de ellos contiene las cartas base del juego, usando nombre de personajes de la serie de Netflix "Castlevania" y habilidades
inspiradas en dichos personajes. El otro archivo debe contener a las cartas editadas por el usuario, osea usted. Está en su
poder modificar también las cartas base del juego, aunque puede romper el juego dejándolo sin cartas base al más mínimo error
de programación que cometa. Por otro lado sus errores de programación se mostrarán al iniciar la aplicación visual.

Eso es todo por ahora...
