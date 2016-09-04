/**/

var createScene = function() {
    var scene = new BABYLON.Scene(engine);
    scene.clearColor = new BABYLON.Color3( .5, .5, .5);

    // camera
    var camera = new BABYLON.ArcRotateCamera("camera1",  0, 0, 0, new BABYLON.Vector3(5, 3, 5), scene);
    camera.setPosition(new BABYLON.Vector3(5, 40, -20));
    camera.alpha = 1.5708;
    camera.beta=-3*Math.PI /20;
    camera.attachControl(canvas, true);
  
    // lights
    var light = new BABYLON.HemisphericLight("light1", new BABYLON.Vector3(1, 0.5, 0), scene);
    light.intensity = 0.8;
    var spot = new BABYLON.SpotLight("spot", new BABYLON.Vector3(25, 15, -10), new BABYLON.Vector3(-1, -0.8, 1), 15, 1, scene);
    spot.diffuse = new BABYLON.Color3(1, 1, 1);
    spot.specular = new BABYLON.Color3(0, 0, 0);
    spot.intensity = 0.2; 
  
    // material
    var mat = new BABYLON.StandardMaterial("mat1", scene);
    mat.alpha = 1.0;
    mat.diffuseColor = new BABYLON.Color3(0.5, 0.5, 1.0);
    mat.backFaceCulling = false;
  
    // Sphere material
    material = new BABYLON.StandardMaterial("kosh5", scene);
    material.diffuseColor = new BABYLON.Color3(0, 0, 0);
    material.reflectionTexture = new BABYLON.CubeTexture("textures/TropicalSunnyDay", scene);
    material.reflectionTexture.level = 0.5;
    material.specularPower = 64;
    material.emissiveColor = new BABYLON.Color3(0.2, 0.2, 0.2);
	
    // Fresnel
    material.emissiveFresnelParameters = new BABYLON.FresnelParameters();
    material.emissiveFresnelParameters.bias = 0.4;
    material.emissiveFresnelParameters.power = 2;
    material.emissiveFresnelParameters.leftColor = BABYLON.Color3.Black();
    material.emissiveFresnelParameters.rightColor = BABYLON.Color3.White();


    // show axis
    var showAxis = function (size) {
	  
        var makeTextPlane = function(text, color, size) {
            var dynamicTexture = new BABYLON.DynamicTexture("DynamicTexture", 50, scene, true);
            dynamicTexture.hasAlpha = true;
            dynamicTexture.drawText(text, 5, 40, "bold 36px Arial", color , "transparent", true);
            var plane = new BABYLON.Mesh.CreatePlane("TextPlane", size, scene, true);
            plane.material = new BABYLON.StandardMaterial("TextPlaneMaterial", scene);
            plane.material.backFaceCulling = false;
            plane.material.specularColor = new BABYLON.Color3(0, 0, 0);
            plane.material.diffuseTexture = dynamicTexture;
            return plane;
        };
  
        var axisX = BABYLON.Mesh.CreateLines("axisX", [ 
          new BABYLON.Vector3.Zero(), new BABYLON.Vector3(size, 0, 0), new BABYLON.Vector3(size * 0.95, 0.05 * size, 0), 
          new BABYLON.Vector3(size, 0, 0), new BABYLON.Vector3(size * 0.95, -0.05 * size, 0)
        ], scene);
        axisX.color = new BABYLON.Color3(1, 0, 0);
        var xChar = makeTextPlane("X", "red", size / 10);
        xChar.position = new BABYLON.Vector3(0.9 * size, -0.05 * size, 0);
        var axisY = BABYLON.Mesh.CreateLines("axisY", [
            new BABYLON.Vector3.Zero(), new BABYLON.Vector3(0, size, 0), new BABYLON.Vector3( -0.05 * size, size * 0.95, 0), 
            new BABYLON.Vector3(0, size, 0), new BABYLON.Vector3( 0.05 * size, size * 0.95, 0)
        ], scene);
        axisY.color = new BABYLON.Color3(0, 1, 0);
        var yChar = makeTextPlane("Y", "green", size / 10);
        yChar.position = new BABYLON.Vector3(0, 0.9 * size, -0.05 * size);
        var axisZ = BABYLON.Mesh.CreateLines("axisZ", [
            new BABYLON.Vector3.Zero(), new BABYLON.Vector3(0, 0, size), new BABYLON.Vector3( 0 , -0.05 * size, size * 0.95),
            new BABYLON.Vector3(0, 0, size), new BABYLON.Vector3( 0, 0.05 * size, size * 0.95)
        ], scene);
        axisZ.color = new BABYLON.Color3(0, 0, 1);
        var zChar = makeTextPlane("Z", "blue", size / 10);
        zChar.position = new BABYLON.Vector3(0, 0.05 * size, 0.9 * size);
    };
  
    size =2;


    //World Local Axes

    var pilot_world_local_axisX = BABYLON.Mesh.CreateLines("pilot_world_local_axisX", [ 
    new BABYLON.Vector3.Zero(), new BABYLON.Vector3(size, 0, 0), new BABYLON.Vector3(size * 0.95, 0.05 * size, 0), 
    new BABYLON.Vector3(size, 0, 0), new BABYLON.Vector3(size * 0.95, -0.05 * size, 0)
    ], scene);
    pilot_world_local_axisX.color = new BABYLON.Color3(1, 0, 0);

    pilot_world_local_axisY = BABYLON.Mesh.CreateLines("pilot_world_local_axisY", [
        new BABYLON.Vector3.Zero(), new BABYLON.Vector3(0, size, 0), new BABYLON.Vector3(-0.05 * size, size * 0.95, 0),
        new BABYLON.Vector3(0, size, 0), new BABYLON.Vector3(0.05 * size, size * 0.95, 0)
    ], scene);
    pilot_world_local_axisY.color = new BABYLON.Color3(0, 1, 0);

    var pilot_world_local_axisZ = BABYLON.Mesh.CreateLines("pilot_world_local_axisZ", [
        new BABYLON.Vector3.Zero(), new BABYLON.Vector3(0, 0, size), new BABYLON.Vector3( 0 , -0.05 * size, size * 0.95),
        new BABYLON.Vector3(0, 0, size), new BABYLON.Vector3( 0, 0.05 * size, size * 0.95)
    ], scene);
    pilot_world_local_axisZ.color = new BABYLON.Color3(0, 0, 1);

    var world_local_origin = new BABYLON.Mesh.CreateBox("wlo", 1, scene);
    world_local_origin.isVisible = false;
	
    pilot_world_local_axisX.parent = world_local_origin;
    pilot_world_local_axisY.parent = world_local_origin;
    pilot_world_local_axisZ.parent = world_local_origin;
    
    var sphere1 = BABYLON.Mesh.CreateSphere("Sphere1", 32, 1, scene);
    var sphere2 = BABYLON.Mesh.CreateSphere("Sphere1", 32, 1, scene);
    sphere1.material = material;
    sphere2.material = material;
	
    //sphere1.position = new BABYLON.Vector3(-0.0880045*2, -0.238661572*2, 1.597247*2);
    //sphere2.position = new BABYLON.Vector3(0.306002438*2, 1.50723732*2, 4.28948545*2);
	
    sphere1.position = new BABYLON.Vector3(-0.0880045, -0.238661572, 1.597247);
    sphere2.position = new BABYLON.Vector3(0.306002438, 1.50723732, 4.28948545);
	
    var pilot = new BABYLON.Mesh.CreateCylinder("pilot", 0.75, 0.2, 0.5, 6, 1 , scene);
    var greyMat = new BABYLON.StandardMaterial("grey", scene);
    greyMat.emissiveColor = new BABYLON.Color3(0.2, 0.2, 0.2);
    pilot.isVisible = false;
    //pilot.material = greyMat;
	  
    var options = {
        width: 10,
        height: 10,
        depth: 1,
    };  
    var arm = new BABYLON.Mesh.CreateBox("arm", 30, scene);
    arm.material = greyMat;
    arm.position.x = 0;
    arm.position.y = 5;
    arm.position.z = 5;
    arm.scaling.x = 1;
    arm.scaling.z = 0.01;
    arm.rotation = new BABYLON.Vector3(-3*Math.PI /20, 0, 0);
	
    arm.parent = pilot;
	
    pilot.position = new BABYLON.Vector3(3, 3, 3);
    world_local_origin.position = new BABYLON.Vector3(0, 0, 0);
    world_local_origin.rotation = new BABYLON.Vector3(-3*Math.PI /20, 0, 0);
	
    //pilot.rotation = new BABYLON.Vector3(0, Math.PI / 2, 0);
    pilot.rotation = new BABYLON.Vector3(Math.PI / 2, 0, 0);
	


    showAxis(10);
  
    return scene;
};
