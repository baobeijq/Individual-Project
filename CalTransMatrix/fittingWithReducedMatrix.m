function [ f, normal, central ] = fittingWithReducedMatrix( pointsPath )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
    fid = fopen(pointsPath, 'r');

    count = 0;
    while ~feof(fid)pointsPath
        line = fgetl(fid);
        if isempty(line)
           break
        end
        count = count + 1;
    end
    fprintf('%d lines in the file\n',count);
    fclose(fid);

    fid = fopen(pointsPath, 'r');
    data = fscanf(fid, '%f %f %f %f %f %f\n', [6, Inf]);
    
    % Diminishing the Size of a Matrix
    if(mod(count,2)~=0)
            data(:, count) = [];
            count=count-1;
    end
    data(:,1:2:count) = [];
    
    %{
    %If the count number is odd, make it even
    if count > 5000
        if(mod(count,2)~=0)
            data(:, count) = [];
            count=count-1;
        end
        data(:,1:2:count) = [];
        if(mod(count/2,2)~=0)
            data(:, count/2) = [];
            count=(count/2)-1;
        else count=count/2;
        end
        data(:,1:2:count) = [];
    
    elseif count > 3000
        if(mod(count,2)~=0)
            data(:, count) = [];
            count=count-1;
        end
        data(:,1:2:count) = [];
    end
    %}
    
        
    
    count2=0;%test
    for n = 1:count/2    % n = 1:count
        point = data(:,n);
    %     if (point(4:4) < 140 || point(4:4) > 160) || (point(5:5) < 50 || point(5:5) > 70)
    %         continue
    %     end
%          fprintf('%d\n', n);
        %scatter3(point(1), point(2), point(3), 'MarkerFaceColor',point(4:6) / 255, 'MarkerEdgeColor',point(4:6) / 255)
        count2 = count2 + 1;%test   RYtgt4:count2= 3335
        hold on
    end
    
    pcloud= [data(1,:)', data(2,:)',data(3,:)'];
    color= [data(4,:)', data(5,:)',data(6,:)'];
    
    f = fit( [data(1,:)', data(2,:)'], data(3,:)', 'poly11' );

    xlabel('x-axis');
    ylabel('y-axis');
    zlabel('z-axis');

    plot (f)
    % hold on

    centralXY = mean(data(1:2,:),2); %one row= 1point,the first and the second point are selected here,mean of each row(x,y)is calculated
    central = [centralXY(1), centralXY(2), f(centralXY(1), centralXY(2))];
    %scatter3(central(1), central(2), central(3), 'MarkerFaceColor',[0,1,0], 'MarkerEdgeColor',[0,1,0])

    P1 = [data(1,1), data(2,1), f(data(1,1), data(2,1))];
    P2 = [data(1,count/2), data(2,count/2), f(data(1,count/2), data(2,count/2))];
    P3 = [data(1,int64(count / 4)), data(2,int64(count / 4)), f(data(1,int64(count / 4)), data(2,int64(count / 4)))];

    % P1 = [0, 0, f(0, 0)];
    % P2 = [0, 1, f(0, 1)];
    % P3 = [1, 0, f(1, 0)];

    normal = cross(P1 - P2, P1 - P3);

    pts = [P1; P3];
    %plot3(pts(:,1), pts(:,2), pts(:,3), 'r'); % r means red --color of line

    pts = [P1; P2];
    %plot3(pts(:,1), pts(:,2), pts(:,3), 'r');

    % pOrigin = [data(1,1), data(2,1), f(data(1,1), data(2,1))];
    % 
    % for n = 1:count
    %     pNow = [data(1,n), data(2,n), f(data(1,n), data(2,n))];
    %     disp(dot((pOrigin - pNow), normal));
    % end

    z = [0, 0, 1];

    %theta = acos(dot(normal, z)/(norm(normal)*norm(z)));

    %zangle = 180 - theta / pi * 180;
    % draw the normal line in 3-d space
    if( abs(normal(1)-normal(2))< 0.001)
        pts = [normal * -50 + central; normal * 50 + central];
    elseif ( abs(normal(1)-normal(2))< 0.005)
        pts = [normal * -10 + central; normal * 10 + central];
    else 
        pts = [normal * -5 + central; normal * 5 + central];
    end
    %{
        RY2furtherReduced: 
        normal =-0.0012    0.0126   -0.0176(display at 5) :0.0138,0.0302
        central = 0.0800   -0.1622    0.9665
        count2 =    1816
    
        RY3furtherReduced: 
        normal = -0.0150   -0.0125    0.0145(display at 10) : 0.0025 0.027   
        count2 =    1672    
    
        RY4furtherReduced: 
        normal = -0.0003   -0.0009    0.0013(display at 10) : 0.00053 0.0022   
        count2 =   1667
    
        B2normal =  -0.0000    0.0158   -0.0092 : 0.0158 0.025
        C2normal =  -0.0006   -0.0255    0.0164 : 0.0249 0.419 
    %}
    
    plot3(pts(:,1), pts(:,2), pts(:,3))

    %hold off
    fclose(fid);

end

  