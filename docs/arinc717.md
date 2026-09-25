# ARINC 717 en bref

ARINC 717 décrit comment les paramètres de vol sont rangés dans le flux enregistré par un FDR (enregistreur de vol) ou un QAR (enregistreur d'accès rapide, utilisé par les compagnies pour l'analyse des vols).

## Le mot de 12 bits

L'unité de base est un mot de **12 bits** (valeurs de 0 à 4095). Dans ce projet, chaque mot est stocké sur 16 bits little-endian ; les 4 bits de poids fort sont ignorés.

## Sous-trame et trame

- Une **sous-trame** = **1 seconde** de vol = un nombre fixe de mots (64, 128, 256, 512 ou 1024 selon l'avion ; 64 ici).
- Une **trame** = **4 sous-trames** = 4 secondes.
- Le premier mot de chaque sous-trame est un **mot de synchronisation** :

| Sous-trame | Mot de synchro |
|---|---|
| 1 | `0x247` |
| 2 | `0x5B8` |
| 3 | `0xA47` |
| 4 | `0xDB8` |

Le décodeur valide une synchro quand il trouve un mot de synchro suivi, exactement une sous-trame plus loin, du mot de synchro suivant.

## Le frame layout

Le flux brut n'est qu'une suite de nombres. Le **layout** indique, pour chaque paramètre :

- **où il se trouve** : numéro de mot (1 = synchro), premier bit (1 = poids faible) et nombre de bits ;
- **dans quelles sous-trames** : partout, ou seulement certaines pour les paramètres lents ;
- **comment le convertir** : codage (`Bnr` ou `Discrete`), signe, résolution et offset.

Formule pour un BNR : `valeur physique = valeur brute × résolution + offset`.

Exemple : altitude sur le mot 2, 12 bits, résolution 8 ft. Une valeur brute de 1250 donne 1250 × 8 = **10 000 ft**.

## Valeurs signées

Un paramètre signé (le tangage par exemple) utilise le **complément à deux** : si le bit de poids fort est à 1, la valeur est négative. Sur 12 bits, `0xFE2` vaut -30, soit -3,0° avec une résolution de 0,1°.
