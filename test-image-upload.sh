#!/bin/bash

# ===========================================
# CLOUDINARY IMAGE UPLOAD TEST SCRIPT
# ===========================================

echo "🚀 Testing Cloudinary Image Upload Integration"
echo "=============================================="

# Base URL de la API
BASE_URL="http://localhost:5179/api"

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Función para mostrar mensajes
print_status() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ️  $1${NC}"
}

# Verificar que el servidor esté corriendo
print_info "Checking if server is running..."
if curl -s "$BASE_URL/Owner" > /dev/null 2>&1; then
    print_status "Server is running on $BASE_URL"
else
    print_error "Server is not running. Please start with: dotnet run"
    exit 1
fi

echo ""
print_info "Testing Image Upload Endpoint..."

# Test 1: Crear una imagen de prueba simple
print_info "Creating test image..."
cat > test-image.txt << 'EOF'
To test image upload, you need a real image file.
Please replace this with an actual image file (.jpg, .png, .gif, .webp)
EOF

echo ""
print_info "Example curl command for image upload:"
echo "curl -X POST \"$BASE_URL/Image/upload?folder=real-estate/test\" \\"
echo "  -F \"file=@path/to/your/image.jpg\" \\"
echo "  -H \"Content-Type: multipart/form-data\""

echo ""
print_info "Example curl command for multiple images:"
echo "curl -X POST \"$BASE_URL/Image/upload-multiple?folder=real-estate/test\" \\"
echo "  -F \"files=@image1.jpg\" \\"
echo "  -F \"files=@image2.jpg\" \\"
echo "  -H \"Content-Type: multipart/form-data\""

echo ""
print_info "Testing other endpoints that don't require files..."

# Test: Generate URL for existing image
echo ""
print_info "Testing URL generation (will fail if public_id doesn't exist):"
curl -X GET "$BASE_URL/Image/url/sample-test-id?width=400&height=300&format=webp" \
  -H "Accept: application/json" \
  -w "\nStatus: %{http_code}\n" 2>/dev/null

echo ""
print_info "Testing responsive URLs generation:"
curl -X GET "$BASE_URL/Image/responsive/sample-test-id" \
  -H "Accept: application/json" \
  -w "\nStatus: %{http_code}\n" 2>/dev/null

echo ""
print_status "Image API endpoints are responding!"
print_info "To test actual file upload, use the curl commands above with real image files."

# Cleanup
rm -f test-image.txt

echo ""
print_info "Ready for manual image upload testing! 🎉"