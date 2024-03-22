An IOS App using unity.
https://apps.apple.com/ro/app/idle-deck-builder/id6449966053

The game is an endless idle game with a set of cards that can be bought to slowly increase profits to further upgrade and buy new cards. the best score achieved is played out each time mana depletes to give passive income and incentivize reaching higher scores. 
The card is balanced through a few values: mana, coins, time, cards. This way any time one is gained another is lost. Each card is balanced in a way to incentivize strategy of ordering of play and deck building. 
It is made to be a simple as possible to be easy to pick up and learn. 
Currently there are only 3 cards, one trades exponential coins for fixed mana (this makes it optimized for mana early, but for coins late), another card trades fixed coins for exponential mana (doing the opposite balance), the final card trades coins for more mana. 
cards can also be bought (at an increasing price) trading cards for coins. mana decreases over time to allow for passive income and to push faster play.

Card: Represents a card in the game. Contains information about the card such as its name, description, image, actions, level, and mana cost.
CardInstance: Represents an instance of a card. Stores a reference to the corresponding Card object and its level. Contains an Upgrade method to upgrade the card instance and manage the upgrade cost.
CardDisplay: Responsible for displaying the card's visual elements such as the image, name, and description. This component is attached to a Card prefab.
Draggable: Allows the player to drag and drop cards. Handles the card's behavior during dragging, such as checking if the card is playable and updating the card's tilt and position.
GameManager: Singleton class that manages the game state and global actions, such as spending bank and mana.
UpgradeCardButton: Handles the interaction of upgrading a card when the upgrade button is clicked. Updates the card's level and upgrade cost.
DeckManager: Manages the player's deck of cards, allowing the player to view and interact with their cards.
