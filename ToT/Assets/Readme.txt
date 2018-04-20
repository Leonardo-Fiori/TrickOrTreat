SPIEGAZIONE FUNZIONAMENTO PROTOTIPO

GameManager prepara il back end instanziando Map, TileSet MovementController e generando la mappa.
Dentro di esso salva tre riferimenti statici a player, map e mover
La mappa quando generata genera anche le tile front end dentro la scena. < DA FARE
MovementManager quando instanziato genera anche il player front end dentro la scena. < DA FARE

Il RaycastManager nell'update rileva il tile su cui viene cliccato. < TUTTO DA FARE
Controlla se GameManager.player.getX e GameManager.player.getY corrispondono a x e y del tile cliccato.
	- Se corrispondono allora chiama la funzione GamaManager.map.getTile(x,y).rotate(bool orario) (back end)
	  Chiama anche la funzione di rotazione del prefab del tile (front end)
	- Altrimenti se non corrispondono controlla se il tile cliccato è uno dei quattro adiacenti al player
		- Se è adiacente chiama la GameMamager.mover.move(Direction dir)
		  Chiama anche la funzione di spostamento el prefab del player (front end)
		- Altrimenti non fa nulla

PER ORA, FINE.

PER IL TESTING USARE LA SCENA PERSONALE (_Domiziano, _Jacopo, _Leonardo)
NON MODIFICARE LA SCENA Main SENZA PRIMA AVVERTIRE E CHIEDERE IL PERMESSO
PUSHARE DI VOLTA IN VOLTA SOLO I FILE NECESSARI CHE SONO STATI MODIFICATI

Il progetto è strutturato su due livelli:
- Back end
- Front end

Il livello front end è la parte del gioco che il giocatore può vedere e con la quale interagisce.

Il livello back end è la parte implementativa del gioco, fatta di classi, vettori, matrici e scriptable object.

I due livelli comunicano tramite un oggetto istanziato (o più di uno) come ad esempio il GameManager.

Il livello back end mantiene lo stato della partita, la posizione il tipo e la rotazione dei tile, il turno, ...

Al livello back end vi sono presenti funzioni di utilità per controllare se uno spostamento è valido, ...

Il livello front end è composto da prefab, ui ed altro.

La mappa, a livello front end, è composta da una griglia di prefab "Tile".

Esiste un gameobject (forse il gamemanager?) in grado di rilevare raycasthit, capisce su quale tile arriva il raycast ed inoltra il "segnale" al back end

Il back end fa i relativi calcoli ed aggiorna il front end quando tutto è ok

I prefab dei tile a livello front end contengono un animazione, che li fa ruotare in modo figo di 90 gradi

Contengono anche shader, altra roba o piccoli script front end (da vedere quando si implementano)

