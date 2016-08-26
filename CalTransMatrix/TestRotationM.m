clear all
close all

load ptc_D3
pt2=pcloud;
load ptc_ColorD3
color2=color;

[ C2f, C2normal, C2central ]=fitting('ptc_D3.txt');
centraledpt2 = bsxfun(@minus, pt2,C2central);
CptRotate = pointCloud(pt2,'Color',color2);
CptRotate.Color(:,:)=color2(:,:);
C2ptC = pointCloud(centraledpt2,'Color',color2);
C2ptC.Color(:,:)=color2(:,:);


%Find the central point of the C2 point cloud(after minuse the cental)
count=880;
f = fit( [centraledpt2(:,1), centraledpt2(:,2)], centraledpt2(:,3), 'poly11' );
centralXY = mean(centraledpt2(:,1:2),2); 
centralC = [centralXY(1), centralXY(2), f(centralXY(1), centralXY(2))];

P1 = [centraledpt2(1,1), centraledpt2(1,2), f(centraledpt2(1,1), centraledpt2(1,2))];
P2 = [centraledpt2(count,1), centraledpt2(count,2), f(centraledpt2(count,1), centraledpt2(count,2))];
P3 = [centraledpt2(int64(count / 2),1), centraledpt2(int64(count / 2),2), f(centraledpt2(int64(count / 2),1), centraledpt2(int64(count / 2),2))];

normal = cross(P1 - P2, P1 - P3);

%NOTES draw out the central and normal to see the reason of wrong transform
normaledCentralC= normal/norm(normal); % Get the normalized point
test=sqrt(sum(normaledCentralC.^2));%test the normalize

% Creat the transformation matrix
 angle=pi/6;
 c=cos(angle);
 s=sin(angle);
 C=1-c;
 x=normaledCentralC(1,1);
 y=normaledCentralC(1,2);
 z=normaledCentralC(1,3);
 
%  A = [cos(pi/6)  0   sin(pi/6) 0;
%       0          1   0         0; 
%       -sin(pi/6) 0   cos(pi/6) 0; 
%       0          0   0         1];
% tform1 = affine3d(A);



M =        [double(x*x*C+c)     double(x*y*C-z*s)   double(x*z*C+y*s)    0;
            double(y*x*C+z*s)   double(y*y*C+c)     double(y*z*C-x*s)    0;
            double(z*x*C-y*s)   double(z*y*C+x*s)   double(z*z*C+c)  0;
            0                   0                   0                1];
tform = affine3d(M);

RotatedC2 = pctransform(CptRotate,tform);


figure
title('Manually rotate')
pcshow(C2ptC, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
xlabel('X (m)')
ylabel('Y (m)')
zlabel('Z (m)')
drawnow
hold on
pcshow(RotatedC2, 'VerticalAxis','Y', 'VerticalAxisDir', 'Down')
drawnow       
       
hold off
       
       
       
       
       