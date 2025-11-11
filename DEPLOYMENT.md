# Deployment Guide

This guide covers various deployment options for the Meta Quest 3D Virtual Hangout Room.

## Quick Start (Local Development)

### Option 1: Python HTTP Server
```bash
cd Nov11-MQ-3Dworld
python3 -m http.server 8000
```
Access at: `http://localhost:8000`

### Option 2: Node.js HTTP Server
```bash
npm start
# or
npx http-server -p 8000
```

### Option 3: VS Code Live Server
1. Install "Live Server" extension
2. Right-click `index.html`
3. Select "Open with Live Server"

## Production Deployment

### GitHub Pages (Recommended for Demo)

1. Push code to GitHub repository
2. Go to repository Settings → Pages
3. Select branch and root folder
4. Save and wait for deployment
5. Access at `https://yourusername.github.io/Nov11-MQ-3Dworld/`

**Note:** GitHub Pages provides HTTPS automatically, which is required for WebXR!

### Netlify

1. Create account at [netlify.com](https://netlify.com)
2. Connect your GitHub repository
3. Set build command: (leave empty)
4. Set publish directory: `/`
5. Deploy!

**Advantages:**
- Automatic HTTPS
- Continuous deployment from git
- CDN distribution
- Free tier available

### Vercel

1. Create account at [vercel.com](https://vercel.com)
2. Import GitHub repository
3. Configure project (defaults work fine)
4. Deploy!

**Advantages:**
- Automatic HTTPS
- Edge network
- Zero configuration
- Free for personal projects

## HTTPS Setup for Local Testing with Meta Quest

WebXR requires HTTPS. Here are options for local HTTPS:

### Option 1: ngrok (Easiest)

```bash
# Install ngrok from https://ngrok.com/download

# Start local server
python3 -m http.server 8000

# In another terminal, create tunnel
ngrok http 8000
```

You'll get an HTTPS URL like `https://abc123.ngrok.io` that you can access from Meta Quest browser.

### Option 2: mkcert (Local SSL)

```bash
# Install mkcert
# macOS
brew install mkcert
# Windows
choco install mkcert

# Create local certificate authority
mkcert -install

# Create certificate
mkcert localhost 127.0.0.1 ::1

# Use with http-server
npx http-server -p 8000 -S -C localhost+2.pem -K localhost+2-key.pem
```

### Option 3: Cloudflare Tunnel

```bash
# Install cloudflared
# Follow instructions at https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/install-and-setup/installation/

# Start tunnel
cloudflared tunnel --url http://localhost:8000
```

## Testing on Meta Quest

1. Set up HTTPS locally (see above)
2. Note your HTTPS URL
3. Put on Meta Quest headset
4. Open Quest Browser
5. Navigate to your HTTPS URL
6. Click "Enter VR Mode"
7. Test features with Quest controllers

## Environment Variables (Optional)

Create a `.env` file for custom configuration:

```env
PORT=8000
NODE_ENV=production
```

## Build Optimization (Advanced)

For production, consider:

### 1. Minification
Use a build tool to minify JavaScript:

```bash
npm install -g terser
terser js/main.js -o js/main.min.js -c -m
```

Update `index.html` to use `main.min.js`

### 2. Asset Optimization
- Compress 3D models with Draco
- Optimize textures with Squoosh
- Use WebP format for images

### 3. CDN Alternatives
If jsdelivr is blocked or slow, use alternatives:

```javascript
// In main.js, change imports to:
import * as THREE from 'https://unpkg.com/three@0.158.0/build/three.module.js';
// or
import * as THREE from 'https://cdn.skypack.dev/three@0.158.0';
```

### 4. Self-Host Three.js
Download Three.js and host locally:

```bash
mkdir -p lib
cd lib
curl -O https://cdn.jsdelivr.net/npm/three@0.158.0/build/three.module.js
# Download other dependencies...
```

Update imports in `main.js`:
```javascript
import * as THREE from './lib/three.module.js';
```

## Docker Deployment (Optional)

Create `Dockerfile`:

```dockerfile
FROM nginx:alpine
COPY . /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

Build and run:
```bash
docker build -t vr-hangout .
docker run -p 8080:80 vr-hangout
```

## Monitoring and Analytics

Consider adding:

1. **Google Analytics** for usage tracking
2. **Sentry** for error monitoring
3. **LogRocket** for session replay

Add to `index.html` before `</head>`:

```html
<!-- Example: Google Analytics -->
<script async src="https://www.googletagmanager.com/gtag/js?id=YOUR-ID"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'YOUR-ID');
</script>
```

## Troubleshooting Deployment

### Issue: WebXR not working
- **Solution:** Ensure HTTPS is enabled
- Check browser console for errors
- Verify Meta Quest browser is updated

### Issue: Models not loading
- **Solution:** Check CORS headers
- Ensure model files are accessible
- Verify file paths are correct

### Issue: Slow loading
- **Solution:** Enable gzip compression
- Use CDN for Three.js
- Optimize 3D models and textures

### Issue: LocalStorage full
- **Solution:** Clear old worlds
- Implement quota checking
- Use IndexedDB for larger storage

## Security Considerations

1. **Content Security Policy**
Add to `index.html`:
```html
<meta http-equiv="Content-Security-Policy" 
      content="default-src 'self' https://cdn.jsdelivr.net; 
               script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; 
               style-src 'self' 'unsafe-inline';">
```

2. **HTTPS Only**
Always use HTTPS in production

3. **Input Validation**
Validate uploaded model files

4. **Rate Limiting**
Implement if adding backend features

## Performance Tips

1. **Lazy Loading**: Load models on demand
2. **Level of Detail**: Use LOD for distant objects
3. **Frustum Culling**: Enabled by default in Three.js
4. **Texture Compression**: Use KTX2 format
5. **Instancing**: For repeated objects

## Backup and Recovery

Save worlds to cloud storage:
```javascript
// Example: Save to backend
async function backupWorld(worldData) {
    await fetch('https://your-api.com/worlds', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(worldData)
    });
}
```

## Support

For deployment issues:
- Check browser console
- Verify HTTPS setup
- Test on desktop first
- Review Three.js documentation
- Check WebXR device API docs

---

**Ready to Deploy?** Choose your preferred method above and get your VR hangout room online!
