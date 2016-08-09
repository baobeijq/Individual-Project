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

pt1 =

   -0.2091    0.1228    3.5530
   -0.1991    0.1226    3.5480
   -0.2274    0.1125    3.5350

adjustedB2ptCloud =

   -0.0123    0.0862    0.1556
   -0.0023    0.0860    0.1506
   -0.0305    0.0759    0.1376

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
%load pc2B
%pt1=pcloud
%load pc2C
%pt2=pcloud

figure(1);
[ B2f, B2normal, B2central ]=fitting('pointcloud 2-B.txt');
%[ B2f, B2normal, B2central ]=fitting('RYtgt.txt');

%B2fptCloud = pointCloud(pt1);
centraledpt1 = bsxfun(@minus, pt1,B2central);
B2ptC = pointCloud(centraledpt1);
%sdv(B2fptCloud);

hold on
[ C2f, C2normal, C2central ]=fitting('pointcloud 2-C-new.txt');
%C2fptCloud = pointCloud(pt2);
centraledpt2 = bsxfun(@minus, pt2,C2central);
C2ptC = pointCloud(centraledpt2);

plot3(centraledpt1(:,1),centraledpt1(:,2),centraledpt1(:,3), 'r.')
plot3(centraledpt2(:,1),centraledpt2(:,2),centraledpt2(:,3),'g.')

hold on
tform = pcregrigid(B2ptC, C2ptC, 'Metric','pointToPlane','Extrapolate', true);
AlignedptC = pctransform(C2ptC,tform);

pz=AlignedptC.Location
%fnew = fit( [pz(1,:)', pz(2,:)'], pz(3,:)', 'poly11' );
%plot(fnew)
plot3(pz(:,1),pz(:,2),pz(:,3),'ko')


%fitting('pointcloud 1-D-new.txt');
hold off