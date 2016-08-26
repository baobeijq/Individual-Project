% Main program to merge the red boards from different Kinects
%
%{
save pc1.mat pcloud
save pc2B.mat pcloud
load pc2B.mat
pt1=pcloud
load pc2C.mat
pt2=pcloud

figure
pz=ptCloudAligned.Location
plot3(pz(:,1),pz(:,2),pz(:,3),'x')
plot3(pz(:,1),pz(:,2),pz(:,3),'kx')

%}
%-------------------------------------------------------------------------%
clear variables
%close all
%clc
%-------------------------------------------------------------------------%
load ptc_C1
pt1=pcloud;
load ptc_ColorC1
color1=color;
load ptc_D1
pt2=pcloud;
load ptc_ColorD1
color2=color;

% Another file load set
%{
load RY2
pt1=pcloud;
load Color2
color1=color;
load RY3
pt2=pcloud;
load Color3
color2=color;
%} 

figure(1);
%Input two point clouds matrixes and creat corresponding ptc obj with color
[ B2f, B2normal, B2central ]=fitting('ptc_C1.txt');
centraledpt1 = bsxfun(@minus, pt1,B2central);
B2ptC = pointCloud(centraledpt1,'Color',color1);
B2ptC.Color(:,:)=color1(:,:);

hold on
[ C2f, C2normal, C2central ]=fitting('ptc_D1.txt');
centraledpt2 = bsxfun(@minus, pt2,C2central);
%CptRotate = pointCloud(pt2,'Color',color2);
C2ptC = pointCloud(centraledpt2,'Color',color2);
C2ptC.Color(:,:)=color2(:,:);

%Find the central point of the C2 point cloud(after minuse the cental)
% f = fit( [centraledpt2(1,:)', centraledpt2(2,:)'], centraledpt2(3,:)', 'poly11' );
% centralXY = mean(centraledpt2(1:2,:),2); 
% centralC = [centralXY(1), centralXY(2), f(centralXY(1), centralXY(2))];
% normaledCentralC= centralC/norm(centralC); % Get the normalized point
%test=sqrt(sum(normaledCentralC.^2));%test the normalize



% Optional
%[B2ptC,Binlier,Boutlier] = pcdenoise(B2ptCnoise);
%[C2ptC,Cinlier,Coutlier] = pcdenoise(C2ptCnoise);

% Apply preprocessing steps to filter the noise by downsampling with a box grid filter 
% and set the size of grid filter to be 0.5cm (1cm=0.01)
gridSize = 0.001;%This value is important
fixed = pcdownsample(B2ptC, 'gridAverage', gridSize);
moving = pcdownsample(C2ptC, 'gridAverage', gridSize);


%figure
%hold on
%plot3(centraledpt1(:,1),centraledpt1(:,2),centraledpt1(:,3), 'r.')
%plot3(centraledpt2(:,1),centraledpt2(:,2),centraledpt2(:,3),'g.')

%TEST:
figure
title('Minus the centre+ denoise')
pcshow(B2ptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow
hold on
pcshow(C2ptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow
%hold off

%hold on
[tform,~,rms] = pcregrigid(moving,fixed, 'Metric','pointToPlane','Extrapolate', true);%pointToPlane
AlignedptC = pctransform(moving,tform);

%C1 transform to B1, need one more rotation
% A = [cos(pi/4), sin(pi/4),  0.3987, 0; 
%     -sin(pi/4), cos(pi/4), -0.0440, 0; 
%             0,         0,   0.9160, 0; 
%             0,         0,   0,      1];
% tformC1 = affine3d(A);

%TEST
B2ptC.Color(1:100,:)=[0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;
    0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;
    0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;
    0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;
    0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;0,0,0;];

%TEST:
%normals = pcnormals(AlignedptC);

figure
pcshow(B2ptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow
hold on

pcshow(AlignedptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
title('Two point clouds')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow

hold off

%-------------------------------------------------------------------------%
% Get xyz matrix 
%{
ptcxyz = zeros(AlignedptC.Count,3);
ptcxyz= AlignedptC.Location(:,:);
dlmwrite('ptcAfterAligne.txt',ptcxyz); 
%}

% Find the center of a xyz matrix + get normal
%{
f = fit( [ptcxyz(:,1)', ptcxyz(:,2)'], ptcxyz(:,3)', 'poly11' );
xlabel('x-axis');
ylabel('y-axis');
zlabel('z-axis');

plot (f)
hold on

centralXY = mean(ptcxyz(:,1:2),2); 
central = [centralXY(1), centralXY(2), f(centralXY(1), centralXY(2))];

ptcWithNormal = horzcat(ptcxyz,normals);
%}

%svd(after alignment)
%{
 [u1,s1,v1]=svd(centraledpt1);
 [u2,s2,v2]=svd(ptcxyz);


 R=v2/v1;
 pt2R=ptcxyz/R ; %ptc matrix after rotation


C2ptCR = pointCloud(pt2R,'Color',color2);
C2ptCR.Color(:,:)=AlignedptC.Color(:,:);
%C2ptCR.Color(:,:)=color2(:,:);

figure
pcshow(C2ptCR, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
title('Rotated point cloud')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow
hold on
pcshow(AlignedptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow
%}

%svd(before alignment)
%{
%svd
[u1,s1,v1]=svd(centraledpt1);
[u2,s2,v2]=svd(centraledpt2);

R=v2/v1;
pt2R=centraledpt2*R ; %ptc matrix after rotation

figure
hold on
plot3(centraledpt1(:,1),centraledpt1(:,2),centraledpt1(:,3), 'rX')
plot3(pt2R(:,1),pt2R(:,2),pt2R(:,3),'bX')
hold off

C2ptCR = pointCloud(pt2R,'Color',color2);
C2ptCR.Color(:,:)=color2(:,:);

figure
pcshow(C2ptCR, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
title('Rotated point cloud')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow
hold on
pcshow(B2ptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow
%} 

%merge
%{
mergeSize = 0.001;
ptCloudScene = pcmerge(B2ptC, AlignedptC, mergeSize);

figure
pcshow(ptCloudScene, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
title('Combined data')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow

{

RYtgt2&RYtgt3
tform.T=
    0.6500    0.0640   -0.7572         0
   -0.0080    0.9970    0.0774         0
    0.7599   -0.0442    0.6485         0
   -0.0069    0.0019    0.0017    1.0000

RYtgt3&RYtgt4
tform.T=
    0.8499   -0.0084    0.5269         0
    0.0208    0.9996   -0.0176         0
   -0.5265    0.0259    0.8498         0
    0.0043   -0.0009    0.0038    1.0000
%} 

%{
C1TOB1:plus anticlockwise 45 degree
tform.T=
    0.8904    0.2195    0.3987         0 x
   -0.2205    0.9744   -0.0440         0 y
   -0.3981   -0.0488    0.9160         0 z
    0.0293   -0.0028   -0.0016    1.0000
random point near center:
x:-0.007357 y:0.007578 z:0.02483

C2TOB2(fig2):
tform.T=
    0.7799    0.3276    0.5333         0
   -0.3314    0.9390   -0.0922         0
   -0.5310   -0.1048    0.8409         0
   -0.0107    0.0051    0.0092    1.0000

D1TOC1(fig1):
tform.T=
    0.2267    0.5214    0.8227         0
   -0.5664    0.7577   -0.3241         0
   -0.7923   -0.3925    0.4671         0
    0.0128   -0.0006   -0.0010    1.0000

D2TOC2:
tform.T=
    0.9994   -0.0238    0.0244         0
    0.0231    0.9994    0.0269         0
   -0.0250   -0.0263    0.9993         0
   -0.0035   -0.0006    0.0009    1.0000
%}

%{ 
LARGE BOARD WITH RIGHT COLOR(SET 3)
C3TOB3:see jpg in the data folder
tform.T=
    0.7009    0.3831    0.6016         0
   -0.3883    0.9125   -0.1286         0
   -0.5983   -0.1435    0.7884         0
   -0.0076    0.0694    0.1117    1.0000

D3&C3 not usable, big difference after tForm, but before the tform they are
quite close

E3ToD3:see jpg in the data folder
tform.T=
   -0.4180    0.5185    0.7459         0
   -0.4943    0.5591   -0.6656         0
   -0.7621   -0.6470    0.0226         0
    0.0530    0.0300    0.0413    1.0000
(feel like no translatio needed)
%}

%{ 
LARGE BOARD WITH RIGHT COLOR (SET1)
C1TOB1:see jpg in the data folder
tform.T=
    0.7957   -0.3882   -0.4650         0
    0.3874    0.9163   -0.1022         0
    0.4657   -0.0988    0.8794         0
    0.0806    0.0230    0.0433    1.0000

D1&C1 perfectly match
tform.T=
    0.5295    0.4623    0.7113         0
   -0.4861    0.8525   -0.1922         0
   -0.6953   -0.2440    0.6761         0
    0.0043    0.0041    0.0062    1.0000

E1ToD1:perfectly match
tform.T=
    0.5121    0.4607    0.7249         0
   -0.4551    0.8613   -0.2260         0
   -0.7285   -0.2142    0.6507         0
    0.0032    0.0162    0.0222    1.0000
(feel like no translatio needed)
%}



