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
%clear variables
%close all
%clc
%-------------------------------------------------------------------------%

%{
B2normal =  -0.0000    0.0158   -0.0092
B2central = -0.1968    0.0366    3.3974
Linear model Poly11:
     B2f(x,y) = p00 + p10*x + p01*y
     Coefficients (with 95% confidence bounds):
       p00 =       3.334  (3.332, 3.336)
       p10 =   -0.002284  (-0.01313, 0.008559)
       p01 =       1.725  (1.707, 1.744)

C2normal =   -0.0006   -0.0255    0.0164
C2central =   0.3698    0.1757    3.5670
Linear model Poly11:
     C2f(x,y) = p00 + p10*x + p01*y
     Coefficients (with 95% confidence bounds):
       p00 =       3.279  (3.275, 3.283)
       p10 =      0.0367  (0.02664, 0.04675)
       p01 =        1.56  (1.541, 1.579)

%}
%-------------------------------------------------------------------------%
load RY3
pt1=pcloud;
load Color3
color1=color;
load RY4
pt2=pcloud;
load Color4
color2=color;

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
[ B2f, B2normal, B2central ]=fitting('RYtgt3.txt');
centraledpt1 = bsxfun(@minus, pt1,B2central);
B2ptC = pointCloud(centraledpt1,'Color',color1);
B2ptC.Color(:,:)=color1(:,:);

hold on
[ C2f, C2normal, C2central ]=fitting('RYtgt4.txt');
centraledpt2 = bsxfun(@minus, pt2,C2central);
C2ptC = pointCloud(centraledpt2,'Color',color2);
C2ptC.Color(:,:)=color2(:,:);

% Apply preprocessing steps to filter the noise by downsampling with a box grid filter 
% and set the size of grid filter to be 0.5cm (1cm=0.01)
gridSize = 0.005;
fixed = pcdownsample(B2ptC, 'gridAverage', gridSize);
moving = pcdownsample(C2ptC, 'gridAverage', gridSize);

%TEST:
figure
pcshow(fixed, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow


%figure
%hold on
%plot3(centraledpt1(:,1),centraledpt1(:,2),centraledpt1(:,3), 'r.')
%plot3(centraledpt2(:,1),centraledpt2(:,2),centraledpt2(:,3),'g.')


hold on
[tform,~,rms] = pcregrigid(moving,fixed, 'Metric','pointToPlane','Extrapolate', true);
AlignedptC = pctransform(moving,tform);

hold on
pcshow(AlignedptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
title('Two point clouds')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow

%pz=AlignedptC.Location
%plot3(pz(:,1),pz(:,2),pz(:,3),'ko')
hold off
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
%}


mergeSize = 0.001;
ptCloudScene = pcmerge(B2ptC, AlignedptC, mergeSize);

figure
pcshow(ptCloudScene, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
title('Combined data')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow

%{

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
hold off




