# Pseudocode

	DrawGeneration
		Apply custom commands
		Apply standard DrawContext commands based on current rule

		If the current rule is a rule-string
			Foreach rule-char in rule-string
				Apply one of time, angle, or length adjustment based on the rule-char
				Recursive DrawGeneration with rule-char, time, angle and length values

		Else if rule-string == "F"
			Draw a segment based on current DrawContext


# Notes

- The current angle, length and time values are specific to a generation, and may be modified during that generation.


# Ideas

- Can angle, length and scale be stored in the DrawContext?
	- Probably not as the effects will cross the generations.
- Pass a GenerationState object when recursing, containing angle, length, time, and scale.