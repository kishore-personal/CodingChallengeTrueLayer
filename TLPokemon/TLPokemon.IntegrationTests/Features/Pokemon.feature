Feature: Pokemon

Scenario Outline: To test if the api endpoint returns pokemon description
	Given the pokemon name is '<pname>'
	When calling the endpoint GetPokemonData
	
	Then I should recieve a pokemon response with description as '<description>' habitat as '<habitat>' isLegendry as '<isLegendary>'
	Examples: 
	| pname   | description                                                                                                    | habitat | isLegendary |
	| mewtwo  | It was created by\na scientist after\nyears of horrificgene splicing and\nDNA engineering\nexperiments.       | rare    | true        |
	| pikachu | When several of\nthese POKéMON\ngather, theirelectricity could\nbuild and cause\nlightning storms.            | forest  | false       |
	| zubat   | Forms colonies in\nperpetually dark\nplaces. Usesultrasonic waves\nto identify and\napproach targets.         | cave    | false       |


Scenario Outline: To test if the api endpoint returns Yoda or Shakespear translated pokemon description 
	Given the pokemon name is '<pname>'
	When calling the endpoint GetTransaltedPokemon
	Then I should recieve a pokemon response with description as '<description>' habitat as '<habitat>' isLegendry as '<isLegendary>'
	Examples: 
	| pname   | description                                                                                                    | habitat | isLegendary |
	| mewtwo  | Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.         | rare    | true        |
	| pikachu | At which hour several of these pokémon gather,  their electricity couldst buildeth and cause lightning storms. | forest  | false       |
	| zubat   | Forms colonies in perpetually dark places.And approach targets,  uses ultrasonic waves to identify.            | cave    | false       |

Scenario Outline: To test if the api endpoint returns pokemon not found 
	Given the pokemon name is '<pname>'
	When calling the endpoint GetPokemonData
	
	Then I should recieve a pokemon response with errorDescription as '<errorDescription>' errorCode as '<errorCode>'
	Examples: 
	| pname | errorDescription      | errorCode |
	| a     | a is not a pokemon    | NotFound  |
	| abcd  | abcd is not a pokemon | NotFound  |
	

	