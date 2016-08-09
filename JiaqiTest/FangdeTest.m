load pc2B
m1=pcloud
load pc2C
m2=pcloud


figure
plot3(m1(:,1),m1(:,2),m1(:,3),'x')
hold on
plot3(m2(:,1),m2(:,2),m2(:,3),'kx')
axis equal
grid on



%move to center

c1=mean(m1,1)
c2=mean(m2,1)

pt1=bsxfun(@minus,m1,c1)
pt2=bsxfun(@minus,m2,c2)



figure
hold on
plot3(pt1(:,1),pt1(:,2),pt1(:,3), 'r.')
plot3(pt2(:,1),pt2(:,2),pt2(:,3),'g.')
axis equal 
grid on
hold on

%rotation caculaiton

cldB = pointCloud(pt1);
cldC=pointCloud(pt2)


[tform,~,rms] = pcregrigid(cldB, cldC, 'Metric','pointToPlane','Extrapolate', true);

aligedC=pctransform(cldB,tform);

ptc2=aligedC.Location


figure
hold on
plot3(pt2(:,1),pt2(:,2),pt2(:,3), 'rX')
plot3(ptc2(:,1),ptc2(:,2),ptc2(:,3),'gX')
axis equal 
grid on
hold on


%svd
[u1,s1,v1]=svd(pt1);
[u2,s2,v2]=svd(pt2);



R=v1/v2
pt1R=pt1*R

figure
hold on
plot3(pt2(:,1),pt2(:,2),pt2(:,3), 'rX')
plot3(pt1R(:,1),pt1R(:,2),pt1R(:,3),'gX')
axis equal 
grid on
hold on


[tform,pt1Al,rms] = pcregrigid(pt1R, pt2, 'Metric','pointToPlane','Extrapolate', true);

figure
hold on
plot3(pt2(:,1),pt2(:,2),pt2(:,3), 'rX')
plot3(pt1Al(:,1),pt1Al(:,2),pt1Al(:,3),'gX')
title('I am the last')
axis equal 
grid on
hold on




