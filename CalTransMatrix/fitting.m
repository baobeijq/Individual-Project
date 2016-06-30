function [ f, normal, central ] = fitting( pointsPath )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
    fid = fopen(pointsPath, 'r');

    count = 0;
    while ~feof(fid)
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
    
    for n = 1:count
        point = data(:,n);
    %     if (point(4:4) < 140 || point(4:4) > 160) || (point(5:5) < 50 || point(5:5) > 70)
    %         continue
    %     end

%          fprintf('%d\n', n);
        scatter3(point(1), point(2), point(3), 'MarkerFaceColor',point(4:6) / 255, 'MarkerEdgeColor',point(4:6) / 255)
        hold on
    end

    f = fit( [data(1,:)', data(2,:)'], data(3,:)', 'poly11' );

    xlabel('x-axis');
    ylabel('y-axis');
    zlabel('z-axis');

    plot (f)
    % hold on

    centralXY = mean(data(1:2,:),2);
    central = [centralXY(1), centralXY(2), f(centralXY(1), centralXY(2))];
    scatter3(central(1), central(2), central(3), 'MarkerFaceColor',[0,1,0], 'MarkerEdgeColor',[0,1,0])

    P1 = [data(1,1), data(2,1), f(data(1,1), data(2,1))];
    P2 = [data(1,count), data(2,count), f(data(1,count), data(2,count))];
    P3 = [data(1,int64(count / 2)), data(2,int64(count / 2)), f(data(1,int64(count / 2)), data(2,int64(count / 2)))];

    % P1 = [0, 0, f(0, 0)];
    % P2 = [0, 1, f(0, 1)];
    % P3 = [1, 0, f(1, 0)];

    normal = cross(P1 - P2, P1 - P3);

    pts = [P1; P3];
    plot3(pts(:,1), pts(:,2), pts(:,3), 'r');

    pts = [P1; P2];
    plot3(pts(:,1), pts(:,2), pts(:,3), 'r');

    % pOrigin = [data(1,1), data(2,1), f(data(1,1), data(2,1))];
    % 
    % for n = 1:count
    %     pNow = [data(1,n), data(2,n), f(data(1,n), data(2,n))];
    %     disp(dot((pOrigin - pNow), normal));
    % end

    z = [0, 0, 1];

    theta = acos(dot(normal, z)/(norm(normal)*norm(z)));

    zangle = 180 - theta / pi * 180;

    pts = [normal * -5 + central; normal * 5 + central];
    plot3(pts(:,1), pts(:,2), pts(:,3))

    hold off
    fclose(fid);

end

