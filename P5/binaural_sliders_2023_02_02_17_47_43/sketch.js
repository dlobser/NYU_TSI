var oscGroup = [];
var baseFreqSlider = [];
var offsetFreqSlider = [];
var oscVolumeSlider = [];
var oscPitch = 400;

function setup() {
  createCanvas(600, 400);
  for (var i = 0; i < 5; i++) {
    oscGroup[i] = [new p5.Oscillator(), new p5.Oscillator()];
    oscGroup[i][0].setType('sine');
    oscGroup[i][1].setType('sine');
    oscGroup[i][0].pan(-1); // pan left
    oscGroup[i][1].pan(1); // pan right
    oscGroup[i][0].start();
    oscGroup[i][1].start();

    baseFreqSlider[i] = createSlider(20, 2000, oscPitch, 1);
    baseFreqSlider[i].position(10, 10 + i * 60);
    offsetFreqSlider[i] = createSlider(0, 50, 5, .01);
    offsetFreqSlider[i].position(150, 10 + i * 60);
    oscVolumeSlider[i] = createSlider(0, 1, 0, 0.01);
    oscVolumeSlider[i].position(290, 10 + i * 60);
    // createP("Group " + (i + 1) + " Base Frequency").position(10, -5 + i * 60);
    // createP("Group " + (i + 1) + " Right Offset Frequency").position(150, -5 + i * 60);
    // createP("Group " + (i + 1) + " Volume").position(290, -5 + i * 60);
  }
}

function draw() {
  background(220);
  for (var i = 0; i < 5; i++) {
    oscGroup[i][0].freq(baseFreqSlider[i].value());
    oscGroup[i][1].freq(baseFreqSlider[i].value() + offsetFreqSlider[i].value());
    oscGroup[i][0].amp(oscVolumeSlider[i].value());
    oscGroup[i][1].amp(oscVolumeSlider[i].value());
  }
  drawSineWave();
}

function randomizeSliders() {
  for (var i = 0; i < 5; i++) {
    baseFreqSlider[i].value(random(20, 1000));
    offsetFreqSlider[i].value(random(0, 50));
    oscVolumeSlider[i].value(random(0, .2));
  }
}

function keyPressed() {
  if (key == 'r') {
    randomizeSliders();
  }
}

function drawSineWave() {
  var x = 0;
  var y = 0;
  var frequencySum = [];
  
  for (var i = 0; i < 5; i++) {
    frequencySum[i] = abs(oscGroup[i][0].freq().value - oscGroup[i][1].freq().value);
  }
    
  push();
  translate(width / 2, height / 2);
  stroke(0);
  strokeWeight(2);
  noFill();
  beginShape();
  for (var x = -width / 2; x < width / 2; x++) {
    let y = 0;
    for (var i = 0; i < 5; i++) {
      y += sin(x * frequencySum[i] / 100) * height / 4 * oscVolumeSlider[i].value();
    }
    vertex(x, y+150);
  }
  endShape();
  pop();
}

