# README #

Alcune istanze di SOAnimation vanno bene per pi� oggetti assieme e contemporaneamente
poich� ricavano valori specifici per l'oggetto all'interno.

Altre istanze di SOAnimation sono dedicate a singoli oggetti poich� alcuni valori
devono essere passati dal codice che da play e potrebbero esserci conflitti se pi� oggetti usano la stessa istanza di SOAnim.

Nel caso in cui l'animazione non abbia bisogno di valori da passare la reference dentro il codice che da play � di tipo generico SOAnim
altrimenti deve essere specificato il tpo (SOPawnMoveAnim per esempio).

Possibile miglioramento:
Non ereditare niente dalla SOAnimation, solo Check e Clean
Implementare di volta in volta la play con numero e tipo di argomenti corretto
I parametri risiedono in chi da play e non nell'animazione
PROBLEMA: se il codice che da play non dichiara la reference all'animazione
con il tipo specifico di figlio di SOAnim, non sar� possinile usare la .Play
poich� non definita nella classe base.