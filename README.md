# Tablet Friend

![logo](icons/logo.png)



Hey there! 



actions:

```yaml
# This is a layout file. It defines what buttons do, how they look and how they are arranged. 
# This may look a bit overwhelming at first, but you can remove any properties that you don't 
# need and everythign will work just fine. For example, you can remove all theme properties 
# and simply use external themes.
# You can also hot-reload the layouts and immediately preview your changes. 
# Simply edit and save the layout, and the app will immediately display your changes.

# IMPORTANT: When editing layouts, use tab characters '	' instead of spaces for indentations.
# Indentations DO matter, they determine the property hierarchy.

# Max layout width measured in cells.
layout_width: 4
# External theme file. Contains the same fileds as 'theme' property. If 'theme' as any properties of its own, they override external theme.
external_theme: files/themes/peach.yaml
# Local theme.
theme:
	# 1x1 button size measured in pixels.
	button_size: 40
	# Space between the buttons measured in pixels.
	margin: 8
	# Determines how round the window will be.
	rounding: 8
	# Default button style. Applied when the button has no 'style' property.
	# Allowed styles:
	# - default
	# - accent
	# - circular_accent
	# - circular
	# - outlined
	# - shy
	# - none
	default_style: default
	# Toolbar opacity. 0 - fully transparent, 1 - fully opaque.
	opacity: 0.8
	
	primary_color: "#fff" # Notice that color value are in quotes, since # is a comment character.
	secondary_color: "#fff"
	background_color: "#fff" 
	
	default_font: consolas
	default_font_size: 14
	# Defines the boldness of the font. Allowed values are from 1 to 999.
	default_font_weight: 500 

buttons:
	text: 
		# - key combo
		# - toggle
		# - wait
		# - cmd
		# - hold
		# - release
		# - layout
		# - type
		action: ctrl+c
		actions: 
			- A
			- wait 500
			- B
		size: 4,1
		text: Chain
		
		style: none
		
		icon: undo
		icon_stretch:
		
		font: consolas
		font_size: 14
		default_font_weight: 500 

	spacer1:
		spacer: true
		size: 4,1


```

