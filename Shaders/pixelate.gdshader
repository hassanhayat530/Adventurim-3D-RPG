shader_type canvas_item;

// Added range hint to prevent invalid values (1-1024)
uniform int pixelSize : hint_range(1, 1024) = 4;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
	// Use built-in texture size directly
	ivec2 size = textureSize(SCREEN_TEXTURE, 0);

	// Calculate grid divisions using floating point
	float xFactor = float(size.x) / float(pixelSize);
	float yFactor = float(size.y) / float(pixelSize);

	// Create pixelated UV coordinates
	vec2 grid_uv = vec2(
		floor(UV.x * xFactor + 0.5) / xFactor,
		floor(UV.y * yFactor + 0.5) / yFactor
	);

	// Sample texture using screen UV for full-screen effect
	COLOR = texture(SCREEN_TEXTURE, grid_uv);
}