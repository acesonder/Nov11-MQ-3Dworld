import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.158.0/build/three.module.js';
import { VRButton } from 'https://cdn.jsdelivr.net/npm/three@0.158.0/examples/jsm/webxr/VRButton.js';
import { GLTFLoader } from 'https://cdn.jsdelivr.net/npm/three@0.158.0/examples/jsm/loaders/GLTFLoader.js';

class VirtualHangoutRoom {
    constructor() {
        this.scene = null;
        this.camera = null;
        this.renderer = null;
        this.clock = new THREE.Clock();
        
        // Drawing system
        this.isDrawing = false;
        this.drawColor = '#ff0000';
        this.drawMode = false;
        this.drawPoints = [];
        this.currentDrawLine = null;
        
        // VR controllers
        this.controller1 = null;
        this.controller2 = null;
        
        // Movement
        this.moveSpeed = 5;
        this.moveForward = false;
        this.moveBackward = false;
        this.moveLeft = false;
        this.moveRight = false;
        this.moveUp = false;
        this.moveDown = false;
        
        // World objects
        this.worldObjects = [];
        this.capturedSpaces = [];
        
        // Loaders
        this.gltfLoader = new GLTFLoader();
        
        this.init();
    }
    
    init() {
        this.setupScene();
        this.setupCamera();
        this.setupRenderer();
        this.setupLighting();
        this.setupEnvironment();
        this.setupVR();
        this.setupControls();
        this.setupEventListeners();
        this.animate();
        
        this.updateStatus('Ready! Use VR or desktop controls to explore.');
    }
    
    setupScene() {
        this.scene = new THREE.Scene();
        this.scene.background = new THREE.Color(0x87ceeb);
        this.scene.fog = new THREE.Fog(0x87ceeb, 50, 500);
    }
    
    setupCamera() {
        this.camera = new THREE.PerspectiveCamera(
            75,
            window.innerWidth / window.innerHeight,
            0.1,
            1000
        );
        this.camera.position.set(0, 1.6, 5);
    }
    
    setupRenderer() {
        this.renderer = new THREE.WebGLRenderer({ antialias: true });
        this.renderer.setPixelRatio(window.devicePixelRatio);
        this.renderer.setSize(window.innerWidth, window.innerHeight);
        this.renderer.xr.enabled = true;
        this.renderer.shadowMap.enabled = true;
        this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
        
        document.getElementById('canvas-container').appendChild(this.renderer.domElement);
    }
    
    setupLighting() {
        // Ambient light
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
        this.scene.add(ambientLight);
        
        // Directional light (sun)
        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
        directionalLight.position.set(50, 100, 50);
        directionalLight.castShadow = true;
        directionalLight.shadow.mapSize.width = 2048;
        directionalLight.shadow.mapSize.height = 2048;
        directionalLight.shadow.camera.left = -100;
        directionalLight.shadow.camera.right = 100;
        directionalLight.shadow.camera.top = 100;
        directionalLight.shadow.camera.bottom = -100;
        this.scene.add(directionalLight);
        
        // Hemisphere light for better outdoor feel
        const hemisphereLight = new THREE.HemisphereLight(0x87ceeb, 0x545454, 0.5);
        this.scene.add(hemisphereLight);
    }
    
    setupEnvironment() {
        // Ground plane (stadium-sized)
        const groundGeometry = new THREE.PlaneGeometry(200, 200);
        const groundMaterial = new THREE.MeshStandardMaterial({ 
            color: 0x3a8c3a,
            roughness: 0.8
        });
        const ground = new THREE.Mesh(groundGeometry, groundMaterial);
        ground.rotation.x = -Math.PI / 2;
        ground.receiveShadow = true;
        this.scene.add(ground);
        
        // Grid helper for scale reference
        const gridHelper = new THREE.GridHelper(200, 50, 0x000000, 0x000000);
        gridHelper.material.opacity = 0.2;
        gridHelper.material.transparent = true;
        this.scene.add(gridHelper);
        
        // Add some sample structures
        this.createSampleStructures();
    }
    
    createSampleStructures() {
        // Create a few buildings/structures for context
        const buildingMaterial = new THREE.MeshStandardMaterial({ color: 0xcccccc });
        
        // Building 1
        const building1 = new THREE.Mesh(
            new THREE.BoxGeometry(10, 15, 10),
            buildingMaterial
        );
        building1.position.set(-30, 7.5, -30);
        building1.castShadow = true;
        building1.receiveShadow = true;
        this.scene.add(building1);
        this.worldObjects.push(building1);
        
        // Building 2
        const building2 = new THREE.Mesh(
            new THREE.BoxGeometry(15, 20, 15),
            buildingMaterial
        );
        building2.position.set(40, 10, -20);
        building2.castShadow = true;
        building2.receiveShadow = true;
        this.scene.add(building2);
        this.worldObjects.push(building2);
        
        // Stadium seating area simulation
        for (let i = 0; i < 8; i++) {
            const seat = new THREE.Mesh(
                new THREE.BoxGeometry(50, 2, 5),
                new THREE.MeshStandardMaterial({ color: 0x8B4513 })
            );
            seat.position.set(0, i * 2.5, -50 - i * 3);
            seat.castShadow = true;
            seat.receiveShadow = true;
            this.scene.add(seat);
            this.worldObjects.push(seat);
        }
        
        // Central platform
        const platform = new THREE.Mesh(
            new THREE.CylinderGeometry(15, 15, 1, 32),
            new THREE.MeshStandardMaterial({ color: 0x4169E1 })
        );
        platform.position.set(0, 0.5, 0);
        platform.castShadow = true;
        platform.receiveShadow = true;
        this.scene.add(platform);
        this.worldObjects.push(platform);
    }
    
    setupVR() {
        // Add VR button
        const vrButton = VRButton.createButton(this.renderer);
        document.getElementById('enter-vr-btn').replaceWith(vrButton);
        vrButton.id = 'enter-vr-btn';
        
        // Setup VR controllers
        this.controller1 = this.renderer.xr.getController(0);
        this.controller1.addEventListener('selectstart', () => this.onSelectStart(this.controller1));
        this.controller1.addEventListener('selectend', () => this.onSelectEnd(this.controller1));
        this.scene.add(this.controller1);
        
        this.controller2 = this.renderer.xr.getController(1);
        this.controller2.addEventListener('selectstart', () => this.onSelectStart(this.controller2));
        this.controller2.addEventListener('selectend', () => this.onSelectEnd(this.controller2));
        this.scene.add(this.controller2);
        
        // Add controller models
        const controllerModelFactory = { createControllerModel: (controller) => {
            const geometry = new THREE.BoxGeometry(0.05, 0.05, 0.2);
            const material = new THREE.MeshStandardMaterial({ color: 0x0066cc });
            return new THREE.Mesh(geometry, material);
        }};
        
        const controllerGrip1 = this.renderer.xr.getControllerGrip(0);
        controllerGrip1.add(controllerModelFactory.createControllerModel(controllerGrip1));
        this.scene.add(controllerGrip1);
        
        const controllerGrip2 = this.renderer.xr.getControllerGrip(1);
        controllerGrip2.add(controllerModelFactory.createControllerModel(controllerGrip2));
        this.scene.add(controllerGrip2);
    }
    
    setupControls() {
        // Mouse controls
        this.raycaster = new THREE.Raycaster();
        this.mouse = new THREE.Vector2();
        
        // Keyboard state
        document.addEventListener('keydown', (e) => this.onKeyDown(e));
        document.addEventListener('keyup', (e) => this.onKeyUp(e));
        document.addEventListener('mousemove', (e) => this.onMouseMove(e));
        document.addEventListener('click', (e) => this.onClick(e));
        
        window.addEventListener('resize', () => this.onWindowResize());
    }
    
    setupEventListeners() {
        document.getElementById('capture-room-btn').addEventListener('click', () => this.captureSpace());
        document.getElementById('draw-mode-btn').addEventListener('click', () => this.toggleDrawMode());
        document.getElementById('erase-mode-btn').addEventListener('click', () => this.clearDrawings());
        document.getElementById('draw-color').addEventListener('change', (e) => {
            this.drawColor = e.target.value;
        });
        document.getElementById('save-world-btn').addEventListener('click', () => this.saveWorld());
        document.getElementById('load-world-btn').addEventListener('click', () => this.loadWorld());
        document.getElementById('clear-world-btn').addEventListener('click', () => this.clearWorld());
        document.getElementById('model-upload').addEventListener('change', (e) => this.loadModel(e));
    }
    
    onKeyDown(event) {
        switch(event.key.toLowerCase()) {
            case 'w': this.moveForward = true; break;
            case 's': this.moveBackward = true; break;
            case 'a': this.moveLeft = true; break;
            case 'd': this.moveRight = true; break;
            case ' ': this.moveUp = true; event.preventDefault(); break;
            case 'shift': this.moveDown = true; break;
        }
    }
    
    onKeyUp(event) {
        switch(event.key.toLowerCase()) {
            case 'w': this.moveForward = false; break;
            case 's': this.moveBackward = false; break;
            case 'a': this.moveLeft = false; break;
            case 'd': this.moveRight = false; break;
            case ' ': this.moveUp = false; break;
            case 'shift': this.moveDown = false; break;
        }
    }
    
    onMouseMove(event) {
        if (!this.renderer.xr.isPresenting) {
            this.mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
            this.mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;
        }
    }
    
    onClick(event) {
        if (this.drawMode && !this.renderer.xr.isPresenting) {
            this.addDrawPoint();
        }
    }
    
    onSelectStart(controller) {
        if (this.drawMode) {
            this.isDrawing = true;
            this.startDrawing(controller);
        }
    }
    
    onSelectEnd(controller) {
        this.isDrawing = false;
        this.currentDrawLine = null;
    }
    
    updateMovement() {
        if (this.renderer.xr.isPresenting) return;
        
        const delta = this.clock.getDelta();
        const moveDistance = this.moveSpeed * delta;
        
        const direction = new THREE.Vector3();
        this.camera.getWorldDirection(direction);
        direction.y = 0;
        direction.normalize();
        
        const right = new THREE.Vector3();
        right.crossVectors(direction, new THREE.Vector3(0, 1, 0)).normalize();
        
        if (this.moveForward) {
            this.camera.position.addScaledVector(direction, moveDistance);
        }
        if (this.moveBackward) {
            this.camera.position.addScaledVector(direction, -moveDistance);
        }
        if (this.moveLeft) {
            this.camera.position.addScaledVector(right, -moveDistance);
        }
        if (this.moveRight) {
            this.camera.position.addScaledVector(right, moveDistance);
        }
        if (this.moveUp) {
            this.camera.position.y += moveDistance;
        }
        if (this.moveDown) {
            this.camera.position.y -= moveDistance;
        }
    }
    
    toggleDrawMode() {
        this.drawMode = !this.drawMode;
        const btn = document.getElementById('draw-mode-btn');
        btn.textContent = this.drawMode ? 'Draw Mode: ON' : 'Draw Mode: OFF';
        btn.style.background = this.drawMode ? '#4CAF50' : '#0066cc';
        this.updateStatus(this.drawMode ? 'Draw mode enabled - Click to draw!' : 'Draw mode disabled');
    }
    
    addDrawPoint() {
        this.raycaster.setFromCamera(this.mouse, this.camera);
        const intersects = this.raycaster.intersectObjects(this.scene.children, true);
        
        if (intersects.length > 0) {
            const point = intersects[0].point;
            this.createDrawPoint(point);
        }
    }
    
    createDrawPoint(position) {
        const geometry = new THREE.SphereGeometry(0.1, 16, 16);
        const material = new THREE.MeshStandardMaterial({ color: this.drawColor });
        const sphere = new THREE.Mesh(geometry, material);
        sphere.position.copy(position);
        sphere.castShadow = true;
        this.scene.add(sphere);
        this.drawPoints.push(sphere);
        
        // Create line between points
        if (this.drawPoints.length > 1) {
            const points = [
                this.drawPoints[this.drawPoints.length - 2].position,
                position
            ];
            const lineGeometry = new THREE.BufferGeometry().setFromPoints(points);
            const lineMaterial = new THREE.LineBasicMaterial({ color: this.drawColor, linewidth: 2 });
            const line = new THREE.Line(lineGeometry, lineMaterial);
            this.scene.add(line);
            this.drawPoints.push(line);
        }
    }
    
    startDrawing(controller) {
        const position = new THREE.Vector3();
        controller.getWorldPosition(position);
        this.createDrawPoint(position);
    }
    
    clearDrawings() {
        this.drawPoints.forEach(obj => {
            this.scene.remove(obj);
            if (obj.geometry) obj.geometry.dispose();
            if (obj.material) obj.material.dispose();
        });
        this.drawPoints = [];
        this.updateStatus('Drawings cleared');
    }
    
    captureSpace() {
        // Simulate space capture by creating a bounding box representation
        const captureData = {
            timestamp: Date.now(),
            position: this.camera.position.clone(),
            objects: this.worldObjects.length,
            bounds: {
                min: new THREE.Vector3(-100, 0, -100),
                max: new THREE.Vector3(100, 50, 100)
            }
        };
        
        this.capturedSpaces.push(captureData);
        
        // Visual feedback
        const boxGeometry = new THREE.BoxGeometry(
            captureData.bounds.max.x - captureData.bounds.min.x,
            captureData.bounds.max.y - captureData.bounds.min.y,
            captureData.bounds.max.z - captureData.bounds.min.z
        );
        const boxMaterial = new THREE.MeshBasicMaterial({
            color: 0x00ff00,
            wireframe: true,
            transparent: true,
            opacity: 0.3
        });
        const captureBox = new THREE.Mesh(boxGeometry, boxMaterial);
        captureBox.position.set(0, 25, 0);
        this.scene.add(captureBox);
        
        // Fade out after 2 seconds
        setTimeout(() => {
            let opacity = 0.3;
            const fadeInterval = setInterval(() => {
                opacity -= 0.05;
                boxMaterial.opacity = opacity;
                if (opacity <= 0) {
                    clearInterval(fadeInterval);
                    this.scene.remove(captureBox);
                    boxGeometry.dispose();
                    boxMaterial.dispose();
                }
            }, 50);
        }, 2000);
        
        this.updateStatus(`Space captured! Total captures: ${this.capturedSpaces.length}`);
    }
    
    saveWorld() {
        const worldData = {
            name: `World_${Date.now()}`,
            timestamp: Date.now(),
            cameraPosition: this.camera.position.toArray(),
            objects: this.worldObjects.map(obj => ({
                type: obj.geometry.type,
                position: obj.position.toArray(),
                rotation: obj.rotation.toArray(),
                scale: obj.scale.toArray(),
                color: obj.material.color.getHex()
            })),
            drawings: this.drawPoints.map(obj => ({
                position: obj.position.toArray(),
                color: obj.material.color.getHex()
            })),
            capturedSpaces: this.capturedSpaces
        };
        
        // Save to localStorage
        const worlds = JSON.parse(localStorage.getItem('vr_worlds') || '[]');
        worlds.push(worldData);
        localStorage.setItem('vr_worlds', JSON.stringify(worlds));
        
        this.updateStatus(`World saved: ${worldData.name}`);
        this.updateWorldList();
    }
    
    loadWorld() {
        const worlds = JSON.parse(localStorage.getItem('vr_worlds') || '[]');
        if (worlds.length === 0) {
            this.updateStatus('No saved worlds found');
            return;
        }
        
        // Load the most recent world
        const worldData = worlds[worlds.length - 1];
        
        // Clear current world
        this.clearWorld(false);
        
        // Restore camera position
        this.camera.position.fromArray(worldData.cameraPosition);
        
        // Restore captured spaces
        this.capturedSpaces = worldData.capturedSpaces || [];
        
        this.updateStatus(`World loaded: ${worldData.name}`);
    }
    
    clearWorld(notify = true) {
        // Remove user-created objects but keep environment
        this.drawPoints.forEach(obj => {
            this.scene.remove(obj);
            if (obj.geometry) obj.geometry.dispose();
            if (obj.material) obj.material.dispose();
        });
        this.drawPoints = [];
        this.capturedSpaces = [];
        
        if (notify) {
            this.updateStatus('World cleared');
        }
    }
    
    updateWorldList() {
        const worlds = JSON.parse(localStorage.getItem('vr_worlds') || '[]');
        const listElement = document.getElementById('world-list');
        
        if (worlds.length === 0) {
            listElement.innerHTML = '<div style="color: #999;">No saved worlds</div>';
            return;
        }
        
        listElement.innerHTML = worlds.map((world, index) => `
            <div class="world-item" data-index="${index}">
                ${world.name}<br>
                <small>${new Date(world.timestamp).toLocaleString()}</small>
            </div>
        `).join('');
        
        // Add click handlers
        document.querySelectorAll('.world-item').forEach(item => {
            item.addEventListener('click', () => {
                const index = parseInt(item.dataset.index);
                this.loadWorldByIndex(index);
            });
        });
    }
    
    loadWorldByIndex(index) {
        const worlds = JSON.parse(localStorage.getItem('vr_worlds') || '[]');
        if (index >= 0 && index < worlds.length) {
            const worldData = worlds[index];
            this.clearWorld(false);
            this.camera.position.fromArray(worldData.cameraPosition);
            this.capturedSpaces = worldData.capturedSpaces || [];
            this.updateStatus(`World loaded: ${worldData.name}`);
        }
    }
    
    loadModel(event) {
        const file = event.target.files[0];
        if (!file) return;
        
        const reader = new FileReader();
        reader.onload = (e) => {
            const arrayBuffer = e.target.result;
            this.gltfLoader.parse(arrayBuffer, '', (gltf) => {
                const model = gltf.scene;
                model.position.set(0, 2, -5);
                model.scale.set(2, 2, 2);
                
                model.traverse((child) => {
                    if (child.isMesh) {
                        child.castShadow = true;
                        child.receiveShadow = true;
                    }
                });
                
                this.scene.add(model);
                this.worldObjects.push(model);
                this.updateStatus('3D model imported successfully!');
            }, (error) => {
                console.error('Error loading model:', error);
                this.updateStatus('Error loading model');
            });
        };
        reader.readAsArrayBuffer(file);
    }
    
    updateStatus(message) {
        document.getElementById('status').textContent = message;
    }
    
    onWindowResize() {
        this.camera.aspect = window.innerWidth / window.innerHeight;
        this.camera.updateProjectionMatrix();
        this.renderer.setSize(window.innerWidth, window.innerHeight);
    }
    
    animate() {
        this.renderer.setAnimationLoop(() => this.render());
    }
    
    render() {
        this.updateMovement();
        this.renderer.render(this.scene, this.camera);
    }
}

// Initialize the application
const app = new VirtualHangoutRoom();
