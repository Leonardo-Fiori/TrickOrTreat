# README #

Alcune istanze di SOAnimation vanno bene per più oggetti assieme e contemporaneamente
poichè ricavano valori specifici per l'oggetto all'interno.

Altre istanze di SOAnimation sono dedicate a singoli oggetti poichè alcuni valori
devono essere passati dal codice che da play e potrebbero esserci conflitti se più oggetti usano la stessa istanza di SOAnim.

Nel caso in cui l'animazione non abbia bisogno di valori da passare la reference dentro il codice che da play è di tipo generico SOAnim
altrimenti deve essere specificato il tpo (SOPawnMoveAnim per esempio).

Possibile miglioramento:
Non ereditare niente dalla SOAnimation, solo Check e Clean
Implementare di volta in volta la play con numero e tipo di argomenti corretto
I parametri risiedono in chi da play e non nell'animazione
PROBLEMA: se il codice che da play non dichiara la reference all'animazione
con il tipo specifico di figlio di SOAnim, non sarà possinile usare la .Play
poichè non definita nella classe base.