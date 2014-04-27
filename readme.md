# Fus Talk

A sort of Yahoo Messenger clone written in C# as one of my homeworks for the
[Application Development on .NET Platform][1] course.

What follows is the documentation I wrote at the time in Romanian.

## Mesaje client spre server

- conectare (utilizator, parola, portulMeu)
	încearcă să se conecteze sau să se înregistreze
- deconectare
	se deconectează
- cerereDePrietenie
	îi spune serverului cui să-i trimită o cerere de prietenie
- lasaText
	lasă un mesaj pentru un prieten deconectat

## Mesaje server spre client

- aiCererePrietenie
	serverul informează clientul că cineva vrea să-i fie prieten. El trebuie să răspundă.
- raspundCererePrietenie
	serverul informează clientul dacă i-a fost acceptată sau nu cererea de prietenie.
- aiPrietenConectat
	serverul informează clientul că un prieten s-a conectat. Acest mesaj e trimis și după ce s-a acceptat o cerere de prietenie
- aiPrietenDeconectat
	... că un prieten s-a deconectat
- aiTextLasat
	... a primit un mesaj de la un prieten cât timp a fost plecat.

## Mesaje client spre client

- text
	trimite un text
- fisier
	trimite un fișier
- fusrodah
	trimite Fus ro dah! (se aude sunetul „Fus ro dah!“ și se izbește fereastra de unul din celelalte trei cadrane ale ecranului când se ajunge la „dah!“)


## Ce face programul

- toată comunicarea se face prin XML.
- toate ferestrele se redimensionează corect
- dacă un utilizator trimite o cerere de prietenie la altul deconectat, acesta o va primi la următoarea conectare
- la fiecare mesaj text, fișier sau Fus ro dah!, se deschide fereastra corespunzătoare dacă ea nu există.
- se poate apăsa Enter direct pentru a se trimite un mesaj.
- când se adauga un mesaj în RichTextBox, mereu se mișcă în jos pentru a fi vizibil mesajul.
- există scurtătura Ctrl+F pentru Fus ro dah!
- parola nu este memorată în clar ci se memorează rezumatul SHA256
- dacă prietenul plecă în mijlocul conversației, utilizatorul va fi anunțat în fereastră (la fel dacă revine)
- din lista de prieteni se vede care este conectat și care nu și se scrie în subsolul ferestrei principale când pleacă și când vine cineva.
- clienții nu trebuie să cunoască adresa serverului, aceasta este o setare implicită a programului.
- serverul de ascultare al utilizatorului se creează pe primul port găsit liber de la un port presetat în sus.
- serverul serializează toate datele despre utilizatori ca să nu trebuiască să se înregistreze din nou, să nu se piardă mesajele lăsate, etc.

[1]: http://www.infoiasi.ro/bin/Programs/CS3104_11
