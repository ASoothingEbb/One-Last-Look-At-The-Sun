#ifndef MIRROR_UV
#define MIRROR_UV

void mirrorUV_float(float2 UV, out float2 Out) {
	if (UV.x < 0.5) {
		Out.x = UV.x;
	}
	else {
		Out.x = 0.5 - (UV.x - 0.5);
	}

	if (UV.y < 0.5) {
		Out.y = UV.y;
	}
	else {
		Out.y = 0.5 - (UV.y - 0.5);
	}

	Out *= 2;
}

#endif